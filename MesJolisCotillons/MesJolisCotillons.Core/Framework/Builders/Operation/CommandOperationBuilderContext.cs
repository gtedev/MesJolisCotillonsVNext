namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Requests;

    public class CommandOperationBuilderContext<TRequest, TCommand, TAdapter> : ICommandOperationBuilderContext<TRequest, TCommand, TAdapter>
            where TRequest : IRequest
            where TCommand : ICommand
            where TAdapter : IAdapter<TRequest>
    {
        private TRequest request;

        private TAdapter adapter;

        public void SetAdapter(TAdapter adapter)
        {
            this.adapter = adapter;
        }

        public void SetRequest(TRequest request)
        {
            this.request = request;
        }

        public TAdapter GetAdapter()
            => this.adapter;

        public TRequest GetRequest()
            => this.request;
    }
}
