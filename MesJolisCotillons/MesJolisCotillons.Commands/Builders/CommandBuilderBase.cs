////namespace MesJolisCotillons.Commands.Builders
////{
////    using MesJolisCotillons.Adapters;
////    using MesJolisCotillons.Commands;
////    using MesJolisCotillons.Contracts.Requests;

////    public abstract class CommandBuilderBase<TAdapter, TCommand, TRequest> : ICommandBuilder<TCommand, TRequest>
////            where TAdapter : class, IAdapter
////            where TCommand : ICommand
////            where TRequest : IRequest
////    {
////        public TCommand Build<TAdapter1>(TAdapter1 adapter, TRequest request)
////            where TAdapter1 : IAdapter<TRequest>
////        {
////            var adapterCasted = adapter as TAdapter;
////            return this.BuildCommandBuilder(adapterCasted, request);
////        }

////        public abstract TCommand BuildCommandBuilder(TAdapter adapter, TRequest request);
////    }
////}
