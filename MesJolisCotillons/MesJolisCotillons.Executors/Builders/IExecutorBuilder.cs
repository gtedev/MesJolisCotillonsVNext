namespace MesJolisCotillons.Executors.Builder
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands;

    public interface IExecutorBuilder<TCommand>
        where TCommand : ICommand
    {
        IEnumerable<IExecutorStep<TCommand>> Build(TCommand command);
    }
}
