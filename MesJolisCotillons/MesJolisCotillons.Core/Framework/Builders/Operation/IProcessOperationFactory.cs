namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Response.Builders;
    using MesJolisCotillons.Validation.Builders;

    public interface IProcessOperationFactory<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        Func<Task<TResponse>> CreateProcessOperation(
            Func<Task<TCommand>> commandGenerator,
            IValidationBuilder<TCommand> validationBuilder,
            IExecutorBuilder<TCommand> executorBuilder,
            IResponseBuilder<TCommand, TResponse> responseBuilder);
    }
}
