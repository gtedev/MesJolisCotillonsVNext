namespace MesJolisCotillons.Executors
{
    using System;
    using MesJolisCotillons.Commands;

    public interface IExecutorStep<in TCommand>
    where TCommand : ICommand
    {
        IExecutor<TCommand> GetExecutor();

        Func<bool> CanRunExecutor { get; }
    }
}
