namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Executor;
    using MesJolisCotillons.Core.Framework.Builders.Validation;
    using MesJolisCotillons.DataAccess.Entities.Context;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Response.Builders;
    using MesJolisCotillons.Response.Builders.Failure;
    using MesJolisCotillons.Validation.Builders;
    using MesJolisCotillons.Validation.Validators;

    public class ProcessOperationFactory<TCommand, TResponse> : IProcessOperationFactory<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        private ISaveDbContext saveDbContext;

        private IValidatorsProcessor<TCommand> validatorsProcessor;

        private IExecutorsProcessor<TCommand> executorsProcessor;

        private IFailureResponseBuilder<TCommand, TResponse> failureReponseBuilder;

        public ProcessOperationFactory(
            ISaveDbContext saveDbContext,
            IValidatorsProcessor<TCommand> validatorProcessor,
            IExecutorsProcessor<TCommand> executorsProcessor,
            IFailureResponseBuilder<TCommand, TResponse> failureReponseBuilder)
        {
            this.saveDbContext = saveDbContext;
            this.validatorsProcessor = validatorProcessor;
            this.executorsProcessor = executorsProcessor;
            this.failureReponseBuilder = failureReponseBuilder;
        }

        public Func<Task<TResponse>> CreateProcessOperation(
            Func<Task<TCommand>> commandGenerator,
            IValidationBuilder<TCommand> validationBuilder,
            IExecutorBuilder<TCommand> executorBuilder,
            IResponseBuilder<TCommand, TResponse> responseBuilder)
        {
            return async () =>
            {
                using (var transaction = this.saveDbContext.BeginTransaction())
                {
                    try
                    {
                        var command = await commandGenerator();
                        var validatedReportResult = ValidationReport<TCommand>.CreateDefaultValidationReport(command);

                        if (validationBuilder != null)
                        {
                            var validatorSteps = validationBuilder.Build();
                            validatedReportResult = this.validatorsProcessor.ProcessValidators(validatorSteps, command);
                        }

                        // using reflection to create Response by passing isValid result
                        // as Success property is an immutable property
                        // Need to cast to `object`, otherwise passing isValid, a boolean value, calls the wrong overload of CreateInstance
                        object isValidParam = validatedReportResult.IsValid;
                        var response = Activator.CreateInstance(typeof(TResponse), isValidParam) as TResponse;

                        if (!validatedReportResult.IsValid)
                        {
                            var failureResponse = this.failureReponseBuilder.Build(response, validatedReportResult);
                            return failureResponse;
                        }

                        if (executorBuilder != null)
                        {
                            var executorSteps = executorBuilder.Build(command);
                            await this.executorsProcessor.ProcessExecutors(executorSteps, command);
                        }

                        var finalResponse = responseBuilder.Build(
                            response,
                            validatedReportResult);

                        transaction.Commit();

                        return finalResponse;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
            };
        }
    }
}
