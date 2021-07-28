namespace MesJolisCotillons.Operations.Product.Create
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands.Builders.Product.Create;
    using MesJolisCotillons.Commands.Product.Create;
    using MesJolisCotillons.Contracts.Requests.Product.Create;
    using MesJolisCotillons.Contracts.Responses.Product.Create;
    using MesJolisCotillons.Core;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Executors.Builders.Product.Create;
    using MesJolisCotillons.Response.Builders.Product.Create;

    public class CreateProductOperation : IOperation<CreateProductRequest, CreateProductResponse>
    {
        private readonly IOperationBuilder<CreateProductRequest, CreateProductResponse, CreateProductCommand> operationBuilder;
        private readonly ICreateProductCommandBuilder commandBuilder;
        private readonly ICreateProductExecutorBuilder executorBuilder;
        private readonly ICreateProductResponseBuilder responseBuilder;

        public CreateProductOperation(
            IOperationBuilder<CreateProductRequest, CreateProductResponse, CreateProductCommand> operationBuilder,
            ICreateProductCommandBuilder commandBuilder,
            ICreateProductExecutorBuilder executorBuilder,
            ICreateProductResponseBuilder responseBuilder)
        {
            this.operationBuilder = operationBuilder;
            this.commandBuilder = commandBuilder;
            this.executorBuilder = executorBuilder;
            this.responseBuilder = responseBuilder;
        }

        public Task<CreateProductResponse> Run(CreateProductRequest request)
        {
            return this.operationBuilder
                    .AddCommandBuilder(this.commandBuilder, request)
                    .AddExecutorBuilder(this.executorBuilder)
                    .AddResponseBuilder(this.responseBuilder)
                    .Build()
                    .Run();
        }
    }
}
