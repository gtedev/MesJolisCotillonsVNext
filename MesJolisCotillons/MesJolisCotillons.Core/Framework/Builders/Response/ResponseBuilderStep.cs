namespace MesJolisCotillons.Core.Framework.Builders.Response
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Core.Framework.Factories;
    using MesJolisCotillons.Response.Builders;
    using MesJolisCotillons.Response.Builders.Default;

    public class ResponseBuilderStep<TCommand, TResponse> : IResponseBuilderStep<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        protected readonly IOperationBuilderContext<TCommand, TResponse> operationBuilderContext;
        protected readonly IOperationBuilderStepFactory<TCommand, TResponse> operationBuilderStepFactory;
        protected readonly IDefaultResponseBuilder<TCommand, TResponse> defaultResponseBuilder;

        public ResponseBuilderStep(
            IOperationBuilderContext<TCommand, TResponse> operationBuilderContext,
            IOperationBuilderStepFactory<TCommand, TResponse> operationBuilderStepFactory,
            IDefaultResponseBuilder<TCommand, TResponse> defaultResponseBuilder)
        {
            this.operationBuilderContext = operationBuilderContext;
            this.operationBuilderStepFactory = operationBuilderStepFactory;
            this.defaultResponseBuilder = defaultResponseBuilder;
        }

        public IOperationBuilderStep<TCommand, TResponse> AddResponseBuilder(IResponseBuilder<TCommand, TResponse> responseBuilder)
        {
            this.operationBuilderContext.SetResponseBuilder(responseBuilder);
            return this.operationBuilderStepFactory.Create(this.operationBuilderContext);
        }

        public IOperationBuilderStep<TCommand, TResponse> AddDefaultResponseBuilder()
        {
            this.operationBuilderContext.SetResponseBuilder(this.defaultResponseBuilder);
            return this.operationBuilderStepFactory.Create(this.operationBuilderContext);
        }
    }
}
