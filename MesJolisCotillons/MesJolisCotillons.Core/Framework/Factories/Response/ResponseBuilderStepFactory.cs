namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Core.Framework.Builders.Response;
    using MesJolisCotillons.Response.Builders.Default;

    public class ResponseBuilderStepFactory<TCommand, TResponse> : IResponseBuilderStepFactory<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        private readonly IOperationBuilderStepFactory<TCommand, TResponse> operationBuilderStepFactory;
        private readonly IDefaultResponseBuilder<TCommand, TResponse> defaultResponseBuilder;

        public ResponseBuilderStepFactory(
            IOperationBuilderStepFactory<TCommand, TResponse> operationBuilderStepFactory,
            IDefaultResponseBuilder<TCommand, TResponse> defaultResponseBuilder)
        {
            this.operationBuilderStepFactory = operationBuilderStepFactory;
            this.defaultResponseBuilder = defaultResponseBuilder;
        }

        public IResponseBuilderStep<TCommand, TResponse> Create(IOperationBuilderContext<TCommand, TResponse> operationBuilderContext)
        {
            return new ResponseBuilderStep<TCommand, TResponse>(
                operationBuilderContext,
                this.operationBuilderStepFactory,
                this.defaultResponseBuilder);
        }
    }
}
