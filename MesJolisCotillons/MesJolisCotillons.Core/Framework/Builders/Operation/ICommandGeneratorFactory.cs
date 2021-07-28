namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Commands.Builders;
    using MesJolisCotillons.Contracts.Requests;

    public interface ICommandGeneratorFactory<TCommand, TRequest>
        where TRequest : IRequest
        where TCommand : ICommand
    {
        Func<Task<TCommand>> CreateCommandGenerator<TAdapter>(
            ICommandBuilder<TCommand, TRequest, TAdapter> commandBuilder,
            TAdapter adapter,
            TRequest request)
            where TAdapter : IAdapter<TRequest>;

        Func<Task<TCommand>> CreateCommandGenerator(
            ICommandBuilder<TCommand, TRequest> commandBuilder,
            TRequest request);
    }
}
