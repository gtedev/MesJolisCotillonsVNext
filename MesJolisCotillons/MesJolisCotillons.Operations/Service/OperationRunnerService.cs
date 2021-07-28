namespace MesJolisCotillons.Operations.Service
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core;
    using MesJolisCotillons.Core.Framework.Factories;

    public class OperationRunnerService : IOperationRunnerService
    {

        private readonly IOperationFactory operationFactory;

        public OperationRunnerService(IOperationFactory operationFactory)
        {
            this.operationFactory = operationFactory;
        }

        public async Task<TResponse> ExecuteOperationAsync<TRequest, TResponse, TOperation>(TRequest request)
        where TRequest : IRequest
        where TResponse : ResponseBase
        where TOperation : IOperation<TRequest, TResponse>
        {
            var operation = this.operationFactory.Create<TRequest, TResponse, TOperation>();
            var response = await operation.Run(request);

            return response;
        }

        public async Task<TResponse> ExecuteOperationAsync<TResponse, TOperation>()
        where TResponse : ResponseBase
        where TOperation : IOperation<TResponse>
        {
            var operation = this.operationFactory.Create<TResponse, TOperation>();
            var response = await operation.Run();

            return response;
        }
    }
}
