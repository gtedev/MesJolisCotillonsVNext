namespace MesJolisCotillons.Operations.Product.Delete
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Adapters.Product.Delete;
    using MesJolisCotillons.Commands.Builders.Product.Delete;
    using MesJolisCotillons.Commands.Product.Delete;
    using MesJolisCotillons.Contracts.Requests.Product.Delete;
    using MesJolisCotillons.Contracts.Responses.Product.Delete;
    using MesJolisCotillons.Core;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Executors.Builders.Product.Delete;
    using MesJolisCotillons.Response.Builders.Product.Delete;
    using MesJolisCotillons.Validation.Builders.Product.Delete;

    public class DeleteProductOperation : IOperation<DeleteProductRequest, DeleteProductResponse>
    {
        private readonly IOperationBuilder<DeleteProductRequest, DeleteProductResponse, DeleteProductCommand> operationBuilder;
        private readonly IDeleteProductAdapter adapter;
        private readonly IDeleteProductCommandBuilder commandBuilder;
        private readonly IDeleteProductValidationBuilder validationBuilder;
        private readonly IDeleteProductExecutorBuilder executorBuilder;
        private readonly IDeleteProductResponseBuilder responseBuilder;

        public DeleteProductOperation(
            IOperationBuilder<DeleteProductRequest, DeleteProductResponse, DeleteProductCommand> operationBuilder,
            IDeleteProductAdapter adapter,
            IDeleteProductCommandBuilder commandBuilder,
            IDeleteProductValidationBuilder validationBuilder,
            IDeleteProductExecutorBuilder executorBuilder,
            IDeleteProductResponseBuilder responseBuilder)
        {
            this.operationBuilder = operationBuilder;
            this.adapter = adapter;
            this.commandBuilder = commandBuilder;
            this.validationBuilder = validationBuilder;
            this.executorBuilder = executorBuilder;
            this.responseBuilder = responseBuilder;
        }

        public Task<DeleteProductResponse> Run(DeleteProductRequest request)
        {
            return this.operationBuilder
                    .AddAdapter(this.adapter, request)
                    .AddCommandBuilder(this.commandBuilder)
                    .AddValidationBuilder(this.validationBuilder)
                    .AddExecutorBuilder(this.executorBuilder)
                    .AddResponseBuilder(this.responseBuilder)
                    .Build()
                    .Run();
        }
    }
}
