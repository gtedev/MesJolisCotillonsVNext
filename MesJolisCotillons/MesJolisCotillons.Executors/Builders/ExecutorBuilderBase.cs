namespace MesJolisCotillons.Executors.Builder
{
    using MesJolisCotillons.Commands;
    using System.Collections.Generic;

    public abstract class ExecutorBuilderBase<TCommand> : IExecutorBuilder<TCommand>
        where TCommand : ICommand
    {
        protected readonly IExecutorStepsBuilder<TCommand> Builder;

        public ExecutorBuilderBase(IExecutorStepsBuilder<TCommand> builder)
        {
            this.Builder = builder;
        }

        IEnumerable<IExecutorStep<TCommand>> IExecutorBuilder<TCommand>.Build(TCommand command)
        {
            return this.Build(command);
        }

        public abstract IEnumerable<IExecutorStep<TCommand>> Build(TCommand command);
    }
}
