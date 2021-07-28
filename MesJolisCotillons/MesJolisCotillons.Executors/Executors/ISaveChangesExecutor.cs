namespace MesJolisCotillons.Executors
{
    using MesJolisCotillons.Commands;

    public interface ISaveChangesExecutor<TCommand> : IExecutor<TCommand>
        where TCommand : ICommand
    {
    }
}
