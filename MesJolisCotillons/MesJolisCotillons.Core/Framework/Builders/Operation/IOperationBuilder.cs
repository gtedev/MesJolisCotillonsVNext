namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Commands.Builders;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Command;
    using MesJolisCotillons.Core.Framework.Builders.Validation;

    /// <summary>
    /// <para>Operation Builder is a class that allows the configuration of the workflow of the operation.</para>
    /// <para> 5 main steps in the following order:</para>
    /// <para> + (1) - Adapter: prepare the data that will be used to build the command. With the help of datas from the Request,
    /// it loads necessary datas from the database. For example, if an email is provided in the Request, it can loads existing user from database.</para>
    /// <para> + (2) - Command: build the command with data from the Adapter.</para>
    /// <para> + (3) - Validation: validation of the command (this step is optional).</para>
    /// <para> + (4) - Execution: if validation passed, execution of the executors. For example: an executor can add some records in database. </para>
    /// <para> + (5) - Response: Return the response from the operation.</para>
    /// </summary>
    /// <typeparam name="TRequest">Type Request.</typeparam>
    /// <typeparam name="TResponse">Type Response.</typeparam>
    /// <typeparam name="TCommand">Type Command.</typeparam>
    public interface IOperationBuilder<TRequest, TResponse, TCommand>
        where TRequest : IRequest
        where TResponse : ResponseBase
        where TCommand : ICommand
    {
        /// <summary>
        /// Add Adapter to the operation to prepare data that will be consumed when building the Command.
        /// </summary>
        /// <typeparam name="TAdapter">Adapter.</typeparam>
        /// <param name="adapter">Adapter is a container that contains prepared / loaded datas from the database.</param>
        /// <param name="request">Request received by endpoint.</param>
        /// <returns>a command builder step.</returns>
        ICommandBuilderStep<TRequest, TResponse, TCommand, TAdapter> AddAdapter<TAdapter>(TAdapter adapter, TRequest request)
            where TAdapter : IAdapter<TRequest>;

        /// <summary>
        /// Add CommandBuilder to decide how to build the command that will be consumed through next steps of operation like Validation or Execution.
        /// </summary>
        /// <param name="commandBuilder">CommandBuilder is the builder that builds the command.</param>
        /// <param name="request">Request received by endpoint.</param>
        /// <returns>a validation builder step.</returns>
        IValidationBuilderStep<TCommand, TResponse> AddCommandBuilder(ICommandBuilder<TCommand, TRequest> commandBuilder, TRequest request);
    }
}
