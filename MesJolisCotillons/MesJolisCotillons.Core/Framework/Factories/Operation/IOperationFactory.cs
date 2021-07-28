namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;

    public interface IOperationFactory
    {
        TOperation Create<TRequest, TResponse, TOperation>()
            where TRequest : IRequest
            where TResponse : IResponse
            where TOperation : IOperation<TRequest, TResponse>;

        TOperation Create<TResponse, TOperation>()
            where TResponse : IResponse
            where TOperation : IOperation<TResponse>;
    }
}
