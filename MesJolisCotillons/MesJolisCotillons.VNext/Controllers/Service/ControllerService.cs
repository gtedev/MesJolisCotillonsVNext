namespace MesJolisCotillons.VNext.Controllers.Service
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core;
    using MesJolisCotillons.DataAccess.Entities.Context;
    using MesJolisCotillons.Operations.Service;
    using MesJolisCotillons.VNext.Controllers.Validation;
    using Microsoft.AspNetCore.Mvc;

    public class ControllerService : IControllerService
    {
        private readonly IOperationRunnerService operationRunnerService;
        private readonly IRequestValidatorService requestValidator;
        private readonly ISaveDbContext saveDbContext;

        public ControllerService(
            IOperationRunnerService operationRunnerService,
            IRequestValidatorService requestValidator,
            ISaveDbContext saveDbContext)
        {
            this.operationRunnerService = operationRunnerService;
            this.requestValidator = requestValidator;
            this.saveDbContext = saveDbContext;
        }

        public async Task<IActionResult> ExecuteOperationAsync<TRequest, TResponse, TOperation>(TRequest request)
            where TRequest : IRequest
            where TResponse : ResponseBase
            where TOperation : IOperation<TRequest, TResponse>
        {
            var isRequestValid = this.requestValidator.Validate(request);
            if (!isRequestValid)
            {
                var invalidResponse = this.requestValidator.GetCustomInvalidResponse(request);
                return new OkObjectResult(invalidResponse);
            }

            var response = await this.operationRunnerService
                .ExecuteOperationAsync<TRequest, TResponse, TOperation>(request);

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> ExecuteOperationAsync<TResponse, TOperation>()
            where TResponse : ResponseBase
            where TOperation : IOperation<TResponse>
        {
            var response = await this.operationRunnerService
                .ExecuteOperationAsync<TResponse, TOperation>();

            return new OkObjectResult(response);
        }
    }
}
