namespace MesJolisCotillons.Operations.Service
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core;

    public interface IOperationRunnerService
    {
        Task<TResponse> ExecuteOperationAsync<TRequest, TResponse, TOperation>(TRequest request)
            where TRequest : IRequest
            where TResponse : ResponseBase
            where TOperation : IOperation<TRequest, TResponse>;

        Task<TResponse> ExecuteOperationAsync<TResponse, TOperation>()
            where TResponse : ResponseBase
            where TOperation : IOperation<TResponse>;
    }
}