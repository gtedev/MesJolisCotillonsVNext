namespace MesJolisCotillons.Core.Framework.Factories
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Runner;

    public interface IRunnerOperationStepFactory<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : IResponse
    {
        IRunnerOperationStep<TCommand, TResponse> Create(Func<Task<TResponse>> operationProcess);
    }
}
