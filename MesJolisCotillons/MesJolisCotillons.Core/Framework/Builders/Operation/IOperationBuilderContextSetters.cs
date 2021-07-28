namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Response.Builders;
    using MesJolisCotillons.Validation.Builders;

    public interface IOperationBuilderContextSetters<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        void SetCommandGenerator(Func<Task<TCommand>> commandGenerator);

        void SetValidationBuilder(IValidationBuilder<TCommand> validationBuilder);

        void SetExecutorBuilder(IExecutorBuilder<TCommand> operationExecutorBuilder);

        void SetResponseBuilder(IResponseBuilder<TCommand, TResponse> responseBuilder);
    }
}
