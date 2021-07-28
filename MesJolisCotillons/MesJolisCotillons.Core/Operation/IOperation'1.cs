namespace MesJolisCotillons.Core
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;

    public interface IOperation<TRequest, TResponse>
        where TRequest : IRequest
        where TResponse : IResponse
    {
        Task<TResponse> Run(TRequest request);
    }
}
