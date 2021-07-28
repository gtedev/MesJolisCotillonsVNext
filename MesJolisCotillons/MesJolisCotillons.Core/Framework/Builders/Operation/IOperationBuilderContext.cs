namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;

    public interface IOperationBuilderContext<TCommand, TResponse> : IOperationBuilderContextSetters<TCommand, TResponse>, IOperationBuilderContextGetters<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
    }
}
