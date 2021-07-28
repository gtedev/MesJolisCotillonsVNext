namespace MesJolisCotillons.Core.Framework.Builders.Command
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Commands.Builders;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Validation;

    public interface ICommandBuilderStep<TRequest, TResponse, TCommand, TAdapter>
        where TRequest : IRequest
        where TResponse : ResponseBase
        where TCommand : ICommand
        where TAdapter : IAdapter<TRequest>
    {
        /// <summary>
        /// Add CommandBuilder to decide how to build the command that will be consumed through next steps of operation like Validation or Execution.
        /// </summary>
        /// <param name="commandBuilder">CommandBuilder is the builder that builds the command.</param>
        /// <returns></returns>
        IValidationBuilderStep<TCommand, TResponse> AddCommandBuilder(ICommandBuilder<TCommand, TRequest, TAdapter> commandBuilder);
    }
}
