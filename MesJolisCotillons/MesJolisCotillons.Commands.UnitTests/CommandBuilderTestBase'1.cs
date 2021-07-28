using MesJolisCotillons.Adapters;
using MesJolisCotillons.Commands.Builders;
using MesJolisCotillons.Contracts.Requests;
using NSubstitute;

namespace MesJolisCotillons.Commands.UnitTests
{
    public abstract class CommandBuilderTestBase<TAdapter, TReq, TCommand, TCommandBuilder>
        where TReq : IRequest, new()
        where TCommand : ICommand
        where TAdapter : class, IAdapter<TReq>
        where TCommandBuilder : ICommandBuilder<TCommand, TReq, TAdapter>
    {
        public CommandBuilderTestBase() : base()
        {
            this.Request = new TReq();
            this.Adapter = Substitute.For<TAdapter>();
        }

        protected TAdapter Adapter { get; set; }

        protected TReq Request { get; set; }

        protected TCommand Command { get; set; }

        protected TCommandBuilder CommandBuilder { get; set; }
    }
}
