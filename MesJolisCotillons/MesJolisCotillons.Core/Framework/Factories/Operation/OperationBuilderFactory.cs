namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Core.Framework.Builders.Operation;

    public class OperationBuilderFactory<TRequest, TCommand> : IOperationBuilderFactory<TRequest, TCommand>
        where TRequest : IRequest
        where TCommand : ICommand
    {
        public ICommandOperationBuilderContext<TRequest, TCommand, TAdapter> Create<TAdapter>()
            where TAdapter : IAdapter<TRequest>
            => new CommandOperationBuilderContext<TRequest, TCommand, TAdapter>();
    }
}
