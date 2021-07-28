namespace MesJolisCotillons.Commands.Builders
{
    using MesJolisCotillons.Contracts.Requests;

    public interface ICommandBuilder<TCommand, TRequest>
        where TCommand : ICommand
        where TRequest : IRequest
    {
        TCommand Build(TRequest request);
    }
}
