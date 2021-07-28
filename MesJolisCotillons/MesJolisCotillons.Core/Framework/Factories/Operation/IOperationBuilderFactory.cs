namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Core.Framework.Builders.Operation;

    public interface IOperationBuilderFactory<TRequest, TCommand>
        where TRequest : IRequest
        where TCommand : ICommand
    {
        ICommandOperationBuilderContext<TRequest, TCommand, TAdapter> Create<TAdapter>()
            where TAdapter : IAdapter<TRequest>;
    }
}
