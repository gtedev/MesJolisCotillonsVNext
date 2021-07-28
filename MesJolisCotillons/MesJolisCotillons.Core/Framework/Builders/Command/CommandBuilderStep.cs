namespace MesJolisCotillons.Core.Framework.Builders.Command
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Commands.Builders;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Core.Framework.Builders.Validation;
    using MesJolisCotillons.Core.Framework.Factories;

    public class CommandBuilderStep<TRequest, TResponse, TCommand, TAdapter> :
        ICommandBuilderStep<TRequest, TResponse, TCommand, TAdapter>
        where TRequest : IRequest
        where TResponse : ResponseBase
        where TCommand : ICommand
        where TAdapter : IAdapter<TRequest>
    {
        private readonly ICommandOperationBuilderContextGetters<TRequest, TCommand, TAdapter> commandOperationBuilderContext;
        private readonly IOperationBuilderContext<TCommand, TResponse> operationBuilderContext;
        private readonly ICommandGeneratorFactory<TCommand, TRequest> commandGeneratorFactory;
        private readonly IValidationBuilderStepFactory<TCommand, TResponse> validationBuilderStepFactory;

        public CommandBuilderStep(
            ICommandOperationBuilderContextGetters<TRequest, TCommand, TAdapter> commandOperationBuilderContext,
            IOperationBuilderContext<TCommand, TResponse> operationBuilderContext,
            ICommandGeneratorFactory<TCommand, TRequest> commandGeneratorFactory,
            IValidationBuilderStepFactory<TCommand, TResponse> validationBuilderStepFactory)
        {
            this.commandOperationBuilderContext = commandOperationBuilderContext;
            this.operationBuilderContext = operationBuilderContext;
            this.commandGeneratorFactory = commandGeneratorFactory;
            this.validationBuilderStepFactory = validationBuilderStepFactory;
        }

        public IValidationBuilderStep<TCommand, TResponse> AddCommandBuilder(ICommandBuilder<TCommand, TRequest, TAdapter> commandBuilder)
        {
            var request = this.commandOperationBuilderContext.GetRequest();
            var adapter = this.commandOperationBuilderContext.GetAdapter();

            var commandGeneratorFn = this.commandGeneratorFactory.CreateCommandGenerator(
                commandBuilder,
                adapter,
                request);

            this.operationBuilderContext.SetCommandGenerator(commandGeneratorFn);
            return this.validationBuilderStepFactory.Create(this.operationBuilderContext);
        }
    }
}
