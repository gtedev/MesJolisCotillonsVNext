namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Executor;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Response.Builders.Default;

    public class ExecutorBuilderStepFactory<TCommand, TResponse> : IExecutorBuilderStepFactory<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        private readonly IResponseBuilderStepFactory<TCommand, TResponse> responseBuilderStepFactory;
        private readonly IOperationBuilderStepFactory<TCommand, TResponse> operationBuilderStepFactory;
        private readonly IDefaultResponseBuilder<TCommand, TResponse> defaultResponseBuilder;

        public ExecutorBuilderStepFactory(
            IResponseBuilderStepFactory<TCommand, TResponse> responseBuilderStepFactory,
            IOperationBuilderStepFactory<TCommand, TResponse> operationBuilderStepFactory,
            IDefaultResponseBuilder<TCommand, TResponse> defaultResponseBuilder)
        {
            this.operationBuilderStepFactory = operationBuilderStepFactory;
            this.responseBuilderStepFactory = responseBuilderStepFactory;
            this.defaultResponseBuilder = defaultResponseBuilder;
        }

        public IExecutorBuilderStep<TCommand, TResponse> Create(IOperationBuilderContext<TCommand, TResponse> operationBuilderContext)
        {
            return new ExecutorBuilderStep<TCommand, TResponse>(
                operationBuilderContext,
                this.responseBuilderStepFactory,
                this.operationBuilderStepFactory,
                this.defaultResponseBuilder);
        }
    }
}
