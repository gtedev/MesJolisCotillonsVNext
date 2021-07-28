namespace MesJolisCotillons.Core.Framework.Factories
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Runner;

    public class RunnerOperationStepFactory<TCommand, TResponse> : IRunnerOperationStepFactory<TCommand, TResponse>
        where TResponse : IResponse
        where TCommand : ICommand
    {
        public IRunnerOperationStep<TCommand, TResponse> Create(Func<Task<TResponse>> operationProcess)
        {
            return new RunnerOperationStep<TCommand, TResponse>(operationProcess);
        }
    }
}
