namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using Autofac;
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Commands.Builders;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Command;
    using MesJolisCotillons.Core.Framework.Builders.Validation;
    using MesJolisCotillons.Core.Framework.Factories;

    public class OperationBuilder<TRequest, TResponse, TCommand> : IOperationBuilder<TRequest, TResponse, TCommand>
        where TRequest : IRequest
        where TResponse : ResponseBase
        where TCommand : ICommand
    {
        private readonly IOperationBuilderFactory<TRequest, TCommand> operationBuilderFactory;
        private readonly IValidationBuilderStepFactory<TCommand, TResponse> validationBuilderStepFactory;
        private readonly IComponentContext componentContext;
        private readonly ICommandGeneratorFactory<TCommand, TRequest> commandGeneratorFactory;
        private readonly IOperationBuilderContext<TCommand, TResponse> operationBuilderContext;

        public OperationBuilder(
            IOperationBuilderFactory<TRequest, TCommand> operationBuilderFactory,
            ICommandGeneratorFactory<TCommand, TRequest> commandGeneratorFactory,
            IValidationBuilderStepFactory<TCommand, TResponse> validationBuilderStepFactory,
            IOperationBuilderContext<TCommand, TResponse> operationBuilderContext,
            IComponentContext componentContext)
        {
            this.operationBuilderFactory = operationBuilderFactory;
            this.validationBuilderStepFactory = validationBuilderStepFactory;
            this.componentContext = componentContext;
            this.commandGeneratorFactory = commandGeneratorFactory;
            this.operationBuilderContext = operationBuilderContext;
        }

        public ICommandBuilderStep<TRequest, TResponse, TCommand, TAdapter> AddAdapter<TAdapter>(TAdapter adapter, TRequest request)
            where TAdapter : IAdapter<TRequest>
        {
            var commandOperationBuilderContext = this.operationBuilderFactory.Create<TAdapter>();

            // not ideal to resolve with Autofac here, but it allows to deal with TAdapter only here.
            // and OperationBuilder is not coupled to TAdapter.
            // We might need a factory for that.
            var commandBuilderStepFactory =
                this.componentContext.Resolve<ICommandBuilderStepFactory<TRequest, TResponse, TCommand, TAdapter>>();

            commandOperationBuilderContext.SetRequest(request);
            commandOperationBuilderContext.SetAdapter(adapter);

            return commandBuilderStepFactory.Create(commandOperationBuilderContext);
        }

        public IValidationBuilderStep<TCommand, TResponse> AddCommandBuilder(ICommandBuilder<TCommand, TRequest> commandBuilder, TRequest request)
        {
            var commandGeneratorFn = this.commandGeneratorFactory.CreateCommandGenerator(
                commandBuilder,
                request);

            this.operationBuilderContext.SetCommandGenerator(commandGeneratorFn);
            return this.validationBuilderStepFactory.Create(this.operationBuilderContext);
        }
    }
}
