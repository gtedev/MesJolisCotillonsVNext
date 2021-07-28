namespace MesJolisCotillons.Executors.Builder
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands;

    public interface IExecutorStepsBuilder<TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        /// Add an executor step in the executors pipeline.
        /// </summary>
        /// <typeparam name="TExecutor">Type executor.</typeparam>
        /// <returns>.</returns>
        IExecutorStepsBuilder<TCommand> AddExecutor<TExecutor>()
         where TExecutor : IExecutor<TCommand>, new();

        /// <summary>
        /// Add an executor step in the executors pipeline.
        /// </summary>
        /// <typeparam name="TExecutor">Type executor.</typeparam>
        /// <param name="executor">Executor.</param>
        /// <returns>.</returns>
        IExecutorStepsBuilder<TCommand> AddExecutor<TExecutor>(TExecutor executor)
            where TExecutor : IExecutor<TCommand>;

        /// <summary>
        /// Add this step to Save changes against the database.
        /// </summary>
        /// <returns>.</returns>
        IExecutorStepsBuilder<TCommand> AddSaveChangesStep();

        /// <summary>
        /// Build the list of executors that will be run.
        /// </summary>
        /// <returns>.</returns>
        IEnumerable<IExecutorStep<TCommand>> Build();
    }
}
