using MesJolisCotillons.Commands;
using MesJolisCotillons.DataAccess.Repositories.Repositories;
using NSubstitute;

namespace MesJolisCotillons.Executors.UnitTests
{
    public abstract class ExecutorTestBase<TCommand, TRepository, TExecutor>
        where TCommand : ICommand
        where TRepository : class, IRepositoryBase
        where TExecutor : IExecutor<TCommand>
    {
        public ExecutorTestBase()
        {
            this.RepositoryMock = Substitute.For<TRepository>();
        }

        protected TExecutor Executor { get; set; }

        protected TCommand Command { get; set; }

        protected TRepository RepositoryMock { get; set; }
    }
}
