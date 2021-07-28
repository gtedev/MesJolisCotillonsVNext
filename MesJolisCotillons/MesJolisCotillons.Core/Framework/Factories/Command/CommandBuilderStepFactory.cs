namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Command;
    using MesJolisCotillons.Core.Framework.Builders.Operation;

    public class CommandBuilderStepFactory<TRequest, TResponse, TCommand, TAdapter> : ICommandBuilderStepFactory<TRequest, TResponse, TCommand, TAdapter>
        where TRequest : IRequest
        where TResponse : ResponseBase
        where TCommand : ICommand
        where TAdapter : IAdapter<TRequest>
    {
        private readonly IValidationBuilderStepFactory<TCommand, TResponse> validationBuilderStepFactory;
        private readonly IOperationBuilderContext<TCommand, TResponse> operationBuilderContext;
        private readonly ICommandGeneratorFactory<TCommand, TRequest> commandGeneratorFactory;

        public CommandBuilderStepFactory(
            IValidationBuilderStepFactory<TCommand, TResponse> validationBuilderStepFactory,
            ICommandGeneratorFactory<TCommand, TRequest> commandGeneratorFactory,
            IOperationBuilderContext<TCommand, TResponse> operationBuilderContext)
        {
            this.validationBuilderStepFactory = validationBuilderStepFactory;
            this.operationBuilderContext = operationBuilderContext;
            this.commandGeneratorFactory = commandGeneratorFactory;
        }

        public ICommandBuilderStep<TRequest, TResponse, TCommand, TAdapter> Create(ICommandOperationBuilderContextGetters<TRequest, TCommand, TAdapter> commandOperationBuilderContext)
        {
            return new CommandBuilderStep<TRequest, TResponse, TCommand, TAdapter>(
                commandOperationBuilderContext,
                this.operationBuilderContext,
                this.commandGeneratorFactory,
                this.validationBuilderStepFactory);
        }
    }
}
