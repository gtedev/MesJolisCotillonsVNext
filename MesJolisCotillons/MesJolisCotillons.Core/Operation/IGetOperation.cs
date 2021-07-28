namespace MesJolisCotillons.Core
{
    using MesJolisCotillons.Contracts.Responses;

    public interface IGetOperation<TResponse> : IOperation<TResponse>
        where TResponse : IResponse
    {
    }
}
