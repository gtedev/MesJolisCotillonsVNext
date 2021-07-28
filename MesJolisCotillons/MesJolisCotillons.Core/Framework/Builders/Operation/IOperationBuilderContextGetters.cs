namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Response.Builders;
    using MesJolisCotillons.Validation.Builders;

    public interface IOperationBuilderContextGetters<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        Func<Task<TCommand>> GetCommandGenerator();

        IValidationBuilder<TCommand> GetValidationBuilder();

        IExecutorBuilder<TCommand> GetExecutorBuilder();

        IResponseBuilder<TCommand, TResponse> GetResponseBuilder();
    }
}
