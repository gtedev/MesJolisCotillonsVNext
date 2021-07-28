namespace MesJolisCotillons.Core.Framework.Builders.Validation
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Executor;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Core.Framework.Builders.Response;
    using MesJolisCotillons.Core.Framework.Factories;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Validation.Builders;

    public class ValidationBuilderStep<TCommand, TResponse> : IValidationBuilderStep<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        private readonly IOperationBuilderContext<TCommand, TResponse> operationBuilderContext;
        private readonly IExecutorBuilderStepFactory<TCommand, TResponse> executorBuilderStepFactory;
        private readonly IResponseBuilderStepFactory<TCommand, TResponse> responseBuilderStepFactory;

        public ValidationBuilderStep(
            IOperationBuilderContext<TCommand, TResponse> operationBuilder,
            IExecutorBuilderStepFactory<TCommand, TResponse> executorBuilderStepFactory,
            IResponseBuilderStepFactory<TCommand, TResponse> responseBuilderStepFactory)
        {
            this.operationBuilderContext = operationBuilder;
            this.executorBuilderStepFactory = executorBuilderStepFactory;
            this.responseBuilderStepFactory = responseBuilderStepFactory;
        }

        public IExecutorBuilderStep<TCommand, TResponse> AddValidationBuilder(IValidationBuilder<TCommand> validationBuilder)
        {
            this.operationBuilderContext.SetValidationBuilder(validationBuilder);
            return this.executorBuilderStepFactory.Create(this.operationBuilderContext);
        }

        public IResponseBuilderStep<TCommand, TResponse> AddExecutorBuilder(IExecutorBuilder<TCommand> operationExecutor)
        {
            this.operationBuilderContext.SetExecutorBuilder(operationExecutor);
            return this.responseBuilderStepFactory.Create(this.operationBuilderContext);
        }
    }
}
