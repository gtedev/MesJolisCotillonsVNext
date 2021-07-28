namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using System;
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Response.Builders;
    using MesJolisCotillons.Validation.Builders;

    public class OperationBuilderContext<TCommand, TResponse> : IOperationBuilderContext<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        private IValidationBuilder<TCommand> validationBuilder;

        private IExecutorBuilder<TCommand> executorBuilder;

        private IResponseBuilder<TCommand, TResponse> responseBuilder;

        private Func<Task<TCommand>> commandGenerator;

        public void SetValidationBuilder(IValidationBuilder<TCommand> validationBuilder)
        {
            this.validationBuilder = validationBuilder;
        }

        public void SetExecutorBuilder(IExecutorBuilder<TCommand> executorBuilder)
        {
            this.executorBuilder = executorBuilder;
        }

        public void SetResponseBuilder(IResponseBuilder<TCommand, TResponse> responseBuilder)
        {
            this.responseBuilder = responseBuilder;
        }

        public void SetCommandGenerator(Func<Task<TCommand>> commandGenerator)
        {
            this.commandGenerator = commandGenerator;
        }

        public Func<Task<TCommand>> GetCommandGenerator()
            => this.commandGenerator;

        public IValidationBuilder<TCommand> GetValidationBuilder()
            => this.validationBuilder;

        public IExecutorBuilder<TCommand> GetExecutorBuilder()
            => this.executorBuilder;

        public IResponseBuilder<TCommand, TResponse> GetResponseBuilder()
            => this.responseBuilder;
    }
}
