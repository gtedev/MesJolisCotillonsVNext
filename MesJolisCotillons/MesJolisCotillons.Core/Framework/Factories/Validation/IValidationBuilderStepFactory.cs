namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Core.Framework.Builders.Validation;

    public interface IValidationBuilderStepFactory<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        IValidationBuilderStep<TCommand, TResponse> Create(IOperationBuilderContext<TCommand, TResponse> operationBuilder);
    }
}
