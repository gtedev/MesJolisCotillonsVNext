namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Commands.Builders;
    using MesJolisCotillons.Contracts.Requests;

    public class CommandGeneratorFactory<TCommand, TRequest> :
        ICommandGeneratorFactory<TCommand, TRequest>
        where TRequest : IRequest
        where TCommand : ICommand
    {
        public Func<Task<TCommand>> CreateCommandGenerator<TAdapter>(
            ICommandBuilder<TCommand, TRequest, TAdapter> commandBuilder,
            TAdapter adapter,
            TRequest request)
            where TAdapter : IAdapter<TRequest>
        {
            return async () =>
            {
                await adapter.Init(request);

                return commandBuilder.Build(adapter, request);
            };
        }

        public Func<Task<TCommand>> CreateCommandGenerator(
            ICommandBuilder<TCommand, TRequest> commandBuilder,
            TRequest request)
        {
            return () =>
            {
                return Task.FromResult(commandBuilder.Build(request));
            };
        }
    }
}
