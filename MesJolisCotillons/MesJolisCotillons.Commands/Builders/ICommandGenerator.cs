namespace MesJolisCotillons.Commands.Builders
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Contracts.Requests;

    public interface ICommandGenerator<TCommand, TRequest, TAdapter>
        where TCommand : ICommand
        where TRequest : IRequest
        where TAdapter : IAdapter<TRequest>
    {
        System.Func<TCommand> CreateGenerator(TAdapter adapter, ICommandBuilder<TCommand, TRequest, TAdapter> commandBuilder);
    }
}
