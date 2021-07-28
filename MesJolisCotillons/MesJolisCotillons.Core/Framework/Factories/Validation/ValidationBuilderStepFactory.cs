namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Core.Framework.Builders.Validation;

    public class ValidationBuilderStepFactory<TCommand, TResponse> : IValidationBuilderStepFactory<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        private readonly IExecutorBuilderStepFactory<TCommand, TResponse> executorBuilderStepFactory;
        private readonly IResponseBuilderStepFactory<TCommand, TResponse> responseBuilderStepFactory;

        public ValidationBuilderStepFactory(
            IExecutorBuilderStepFactory<TCommand, TResponse> executorBuilderStepFactory,
            IResponseBuilderStepFactory<TCommand, TResponse> responseBuilderStepFactory)
        {
            this.executorBuilderStepFactory = executorBuilderStepFactory;
            this.responseBuilderStepFactory = responseBuilderStepFactory;
        }

        IValidationBuilderStep<TCommand, TResponse> IValidationBuilderStepFactory<TCommand, TResponse>.Create(IOperationBuilderContext<TCommand, TResponse> operationBuilderContext)
        {
            return new ValidationBuilderStep<TCommand, TResponse>(
                operationBuilderContext,
                this.executorBuilderStepFactory,
                this.responseBuilderStepFactory);
        }
    }
}
