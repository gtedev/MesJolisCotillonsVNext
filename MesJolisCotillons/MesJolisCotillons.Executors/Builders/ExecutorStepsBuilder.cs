namespace MesJolisCotillons.Executors.Builder
{
    using System;
    using System.Collections.Generic;
    using MesJolisCotillons.Commands;

    public class ExecutorStepsBuilder<TCommand> : IExecutorStepsBuilder<TCommand>
        where TCommand : ICommand
    {
        private List<IExecutorStep<TCommand>> executors = new List<IExecutorStep<TCommand>>();
        private readonly ISaveChangesExecutor<TCommand> saveChangesExecutor;

        public ExecutorStepsBuilder(ISaveChangesExecutor<TCommand> saveChangesExecutor)
            => this.saveChangesExecutor = saveChangesExecutor;

        public IExecutorStepsBuilder<TCommand> AddExecutor<TExecutor>()
            where TExecutor : IExecutor<TCommand>, new()
        {
            var executor = new TExecutor();
            return this.AddExecutorStep(() => true, executor);
        }

        public IExecutorStepsBuilder<TCommand> AddExecutor<TExecutor>(TExecutor executor)
            where TExecutor : IExecutor<TCommand>
        {
            return this.AddExecutorStep(() => true, executor);
        }

        public IExecutorStepsBuilder<TCommand> AddSaveChangesStep()
        {
            return this.AddExecutorStep(() => true, this.saveChangesExecutor);
        }

        public IEnumerable<IExecutorStep<TCommand>> Build()
        {
            return this.executors;
        }

        private IExecutorStepsBuilder<TCommand> AddExecutorStep(Func<bool> canRunExecutorFn, IExecutor<TCommand> executor)
        {
            var executorStep = new ExecutorStep<TCommand>(canRunExecutorFn, executor);
            this.executors.Add(executorStep);
            return this;
        }
    }
}
