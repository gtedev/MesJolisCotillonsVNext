namespace MesJolisCotillons.VNext.DI.Autofac.Factories
{
    using global::Autofac;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core;
    using MesJolisCotillons.Core.Framework.Factories;

    public class OperationFactory : IOperationFactory
    {
        private readonly IComponentContext context;

        public OperationFactory(IComponentContext context)
        {
            this.context = context;
        }

        public TOperation Create<TRequest, TResponse, TOperation>()
            where TRequest : IRequest
            where TResponse : IResponse
            where TOperation : IOperation<TRequest, TResponse>
        {
            var operation = this.context.Resolve<TOperation>();
            return operation;
        }

        public TOperation Create<TResponse, TOperation>()
            where TResponse : IResponse
            where TOperation : IOperation<TResponse>
        {
            var operation = this.context.Resolve<TOperation>();
            return operation;
        }
    }
}
