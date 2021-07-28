namespace MesJolisCotillons.Commands.Builders
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Contracts.Requests;

    public interface ICommandBuilder<TCommand, TRequest, TAdapter>
        where TCommand : ICommand
        where TRequest : IRequest
        where TAdapter : IAdapter<TRequest>
    {
        TCommand Build(TAdapter adapter, TRequest request);
    }
}
