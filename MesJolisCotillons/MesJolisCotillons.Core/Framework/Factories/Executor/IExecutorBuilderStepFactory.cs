namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Executor;
    using MesJolisCotillons.Core.Framework.Builders.Operation;

    public interface IExecutorBuilderStepFactory<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        IExecutorBuilderStep<TCommand, TResponse> Create(IOperationBuilderContext<TCommand, TResponse> operationBuilder);
    }
}
