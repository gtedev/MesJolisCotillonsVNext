namespace MesJolisCotillons.Core.Framework.Builders.Validation
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Executor;
    using MesJolisCotillons.Core.Framework.Builders.Response;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Validation.Builders;

    public interface IValidationBuilderStep<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        /// <summary>
        /// Add ValidationBuilder to provide set of validation to check during the Operation.
        /// </summary>
        /// <param name="validationBuilder">Validation builder.</param>
        /// <returns>.</returns>
        IExecutorBuilderStep<TCommand, TResponse> AddValidationBuilder(IValidationBuilder<TCommand> validationBuilder);

        /// <summary>
        /// Add an Executor Builder to perform database query / changes during the operation.
        /// </summary>
        /// <param name="operationExecutor">Executor builder.</param>
        /// <returns>.</returns>
        IResponseBuilderStep<TCommand, TResponse> AddExecutorBuilder(IExecutorBuilder<TCommand> operationExecutor);
    }
}
