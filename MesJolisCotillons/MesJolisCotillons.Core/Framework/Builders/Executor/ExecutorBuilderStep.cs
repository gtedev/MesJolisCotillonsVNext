namespace MesJolisCotillons.Core.Framework.Builders.Executor
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Core.Framework.Builders.Response;
    using MesJolisCotillons.Core.Framework.Factories;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Response.Builders.Default;

    /// <summary>
    /// Executor step builder gives as well the ability to step direcly to Response Builder.
    /// </summary>
    /// <typeparam name="TCommand">Command parameter.</typeparam>
    /// <typeparam name="TResponse">Request parameter.</typeparam>
    public class ExecutorBuilderStep<TCommand, TResponse> : ResponseBuilderStep<TCommand, TResponse>, IExecutorBuilderStep<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        private readonly IResponseBuilderStepFactory<TCommand, TResponse> responseBuilderStepFactory;

        public ExecutorBuilderStep(
            IOperationBuilderContext<TCommand, TResponse> operationBuilderContext,
            IResponseBuilderStepFactory<TCommand, TResponse> responseBuilderStepFactory,
            IOperationBuilderStepFactory<TCommand, TResponse> operationBuilderStepFactory,
            IDefaultResponseBuilder<TCommand, TResponse> defaultResponseBuilder)
            : base(operationBuilderContext, operationBuilderStepFactory, defaultResponseBuilder)
        {
            this.responseBuilderStepFactory = responseBuilderStepFactory;
        }

        public IResponseBuilderStep<TCommand, TResponse> AddExecutorBuilder(IExecutorBuilder<TCommand> operationExecutor)
        {
            this.operationBuilderContext.SetExecutorBuilder(operationExecutor);
            return this.responseBuilderStepFactory.Create(this.operationBuilderContext);
        }
    }
}
