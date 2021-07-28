namespace MesJolisCotillons.Core.Framework.Factories
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Operation;

    public class OperationBuilderStepFactory<TCommand, TResponse> : IOperationBuilderStepFactory<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        private readonly IProcessOperationFactory<TCommand, TResponse> processOperationFactory;
        private readonly IRunnerOperationStepFactory<TCommand, TResponse> runnerOperationStepFactory;

        public OperationBuilderStepFactory(
            IProcessOperationFactory<TCommand, TResponse> processOperationFactory,
            IRunnerOperationStepFactory<TCommand, TResponse> runnerOperationStepFactory)
        {
            this.processOperationFactory = processOperationFactory;
            this.runnerOperationStepFactory = runnerOperationStepFactory;
        }

        public IOperationBuilderStep<TCommand, TResponse> Create(IOperationBuilderContext<TCommand, TResponse> operationBuilderContext)
        {
            return new OperationBuilderStep<TCommand, TResponse>(
                this.processOperationFactory,
                operationBuilderContext,
                this.runnerOperationStepFactory);
        }
    }
}
