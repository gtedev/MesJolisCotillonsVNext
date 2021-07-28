namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Requests;

    public interface ICommandOperationBuilderContext<TRequest, TCommand, TAdapter> :
        ICommandOperationBuilderContextSetters<TRequest, TCommand, TAdapter>,
        ICommandOperationBuilderContextGetters<TRequest, TCommand, TAdapter>
            where TRequest : IRequest
            where TCommand : ICommand
            where TAdapter : IAdapter<TRequest>
    {
    }
}
