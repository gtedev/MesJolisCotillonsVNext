namespace MesJolisCotillons.Executors
{
    using System;
    using MesJolisCotillons.Commands;

    public class ExecutorStep<TCommand> : IExecutorStep<TCommand>
        where TCommand : ICommand
    {
        private IExecutor<TCommand> executor;

        public ExecutorStep(Func<bool> canRunExecutorFn, IExecutor<TCommand> executor)
        {
            this.CanRunExecutor = canRunExecutorFn;
            this.executor = executor;
        }

        public Func<bool> CanRunExecutor { get; }

        public IExecutor<TCommand> GetExecutor() => this.executor;
    }
}
