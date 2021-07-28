namespace MesJolisCotillons.Core.Framework.Builders.Executor
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Response;
    using MesJolisCotillons.Executors.Builder;

    public interface IExecutorBuilderStep<TCommand, TResponse> : IResponseBuilderStep<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase

    {
        /// <summary>
        /// Add ExecutionBuilder to provide sets of executors to run (eventually against the database) when the validation has passed successfully.
        /// </summary>
        /// <param name="operationExecutor"></param>
        /// <returns></returns>
        IResponseBuilderStep<TCommand, TResponse> AddExecutorBuilder(IExecutorBuilder<TCommand> operationExecutor);
    }
}
