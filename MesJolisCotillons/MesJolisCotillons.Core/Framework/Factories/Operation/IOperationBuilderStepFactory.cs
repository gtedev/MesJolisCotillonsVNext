namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Operation;

    public interface IOperationBuilderStepFactory<TCommand, TResponse>
        where TResponse : ResponseBase
        where TCommand : ICommand
    {
        IOperationBuilderStep<TCommand, TResponse> Create(IOperationBuilderContext<TCommand, TResponse> operationBuilder);
    }
}
