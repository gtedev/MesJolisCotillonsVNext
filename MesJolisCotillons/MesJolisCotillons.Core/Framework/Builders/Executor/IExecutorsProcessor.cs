namespace MesJolisCotillons.Core.Framework.Builders.Executor
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Executors;

    public interface IExecutorsProcessor<TCommand>
        where TCommand : ICommand
    {
        Task ProcessExecutors(IEnumerable<IExecutorStep<TCommand>> executors, TCommand command);
    }
}
