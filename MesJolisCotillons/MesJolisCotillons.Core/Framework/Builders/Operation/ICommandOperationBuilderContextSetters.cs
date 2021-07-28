namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Requests;

    public interface ICommandOperationBuilderContextSetters<TRequest, TCommand, TAdapter>
            where TRequest : IRequest
            where TCommand : ICommand
            where TAdapter : IAdapter<TRequest>
    {
        void SetRequest(TRequest request);

        void SetAdapter(TAdapter adapter);
    }
}
