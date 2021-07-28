namespace MesJolisCotillons.Core.Framework.Builders.Runner
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;

    public class RunnerOperationStep<TCommand, TResponse> : IRunnerOperationStep<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : IResponse
    {
        private readonly Func<Task<TResponse>> operationProcess;

        public RunnerOperationStep(Func<Task<TResponse>> operationProcess)
        {
            this.operationProcess = operationProcess;
        }

        public Task<TResponse> Run()
        {
            return this.operationProcess();
        }
    }
}
