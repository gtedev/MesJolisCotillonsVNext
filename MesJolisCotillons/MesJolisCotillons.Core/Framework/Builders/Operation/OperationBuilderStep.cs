namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Runner;
    using MesJolisCotillons.Core.Framework.Factories;

    public class OperationBuilderStep<TCommand, TResponse> : IOperationBuilderStep<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        private readonly IProcessOperationFactory<TCommand, TResponse> processOperationFactory;
        private readonly IOperationBuilderContextGetters<TCommand, TResponse> operationBuilderContext;
        private readonly IRunnerOperationStepFactory<TCommand, TResponse> runnerOperationStepFactory;

        public OperationBuilderStep(
            IProcessOperationFactory<TCommand, TResponse> processOperationFactory,
            IOperationBuilderContextGetters<TCommand, TResponse> operationBuilderContext,
            IRunnerOperationStepFactory<TCommand, TResponse> runnerOperationStepFactory)
        {
            this.processOperationFactory = processOperationFactory;
            this.operationBuilderContext = operationBuilderContext;
            this.runnerOperationStepFactory = runnerOperationStepFactory;
        }

        public IRunnerOperationStep<TCommand, TResponse> Build()
        {
            var commandGenerator = this.operationBuilderContext.GetCommandGenerator();
            var validationBuilder = this.operationBuilderContext.GetValidationBuilder();
            var executionBuilder = this.operationBuilderContext.GetExecutorBuilder();
            var responseBuilder = this.operationBuilderContext.GetResponseBuilder();

            var processOperationFn = this.processOperationFactory.CreateProcessOperation(
                commandGenerator,
                validationBuilder,
                executionBuilder,
                responseBuilder);

            return this.runnerOperationStepFactory.Create(processOperationFn);
        }
    }
}
