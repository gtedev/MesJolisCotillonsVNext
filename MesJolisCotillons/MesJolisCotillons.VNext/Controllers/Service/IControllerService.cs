namespace MesJolisCotillons.VNext.Controllers.Service
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core;
    using Microsoft.AspNetCore.Mvc;

    public interface IControllerService
    {
        Task<IActionResult> ExecuteOperationAsync<TRequest, TResponse, TOperation>(TRequest request)
            where TRequest : IRequest
            where TResponse : ResponseBase
            where TOperation : IOperation<TRequest, TResponse>;

        Task<IActionResult> ExecuteOperationAsync<TResponse, TOperation>()
            where TResponse : ResponseBase
            where TOperation : IOperation<TResponse>;
    }
}
