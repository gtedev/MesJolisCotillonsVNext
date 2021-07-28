namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Command;
    using MesJolisCotillons.Core.Framework.Builders.Operation;

    public interface ICommandBuilderStepFactory<TRequest, TResponse, TCommand, TAdapter>
        where TRequest : IRequest
        where TResponse : ResponseBase
        where TCommand : ICommand
        where TAdapter : IAdapter<TRequest>

    {
        ICommandBuilderStep<TRequest, TResponse, TCommand, TAdapter> Create(ICommandOperationBuilderContextGetters<TRequest, TCommand, TAdapter> commandOperationBuilderContext);
    }
}
