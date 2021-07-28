namespace MesJolisCotillons.Core
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;

    public class OperationRunnable<TRequest, TResponse> : IOperation<TRequest, TResponse>
        where TRequest : IRequest
        where TResponse : IResponse
    {
        protected Func<TRequest, Task<TResponse>> operationRun;

        public OperationRunnable(Func<TRequest, Task<TResponse>> operationRun)
        {
            this.operationRun = operationRun;
        }

        public Task<TResponse> Run(TRequest request) => this.operationRun(request);
    }
}
