namespace MesJolisCotillons.Core.Framework.Builders.Executor
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Executors;

    public class ExecutorsProcessor<TCommand> : IExecutorsProcessor<TCommand>
        where TCommand : ICommand
    {
        public async Task ProcessExecutors(IEnumerable<IExecutorStep<TCommand>> executorSteps, TCommand command)
        {
            foreach (var execStep in executorSteps)
            {
                if (execStep.CanRunExecutor())
                {
                    var exectutor = execStep.GetExecutor();
                    await exectutor.ExecuteAsync(command);
                }
            }
        }
    }
}