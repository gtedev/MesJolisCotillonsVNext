using MesJolisCotillons.Commands.Builders;
using MesJolisCotillons.Contracts.Requests;

namespace MesJolisCotillons.Commands.UnitTests
{
    public abstract class CommandBuilderTestBase<TReq, TCommand, TCommandBuilder>
        where TReq : class, IRequest, new()
        where TCommand : ICommand
        where TCommandBuilder : ICommandBuilder<TCommand, TReq>
    {
        public CommandBuilderTestBase()
        {
            this.Request = new TReq();
        }

        protected TReq Request { get; set; }

        protected TCommand Command { get; set; }

        protected TCommandBuilder CommandBuilder { get; set; }
    }
}
