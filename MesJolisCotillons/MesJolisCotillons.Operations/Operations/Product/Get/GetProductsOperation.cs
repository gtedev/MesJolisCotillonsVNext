namespace MesJolisCotillons.Operations.Product.Get
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Adapters.Product.Get;
    using MesJolisCotillons.Commands.Builders.Product.Get;
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.Contracts.Requests.Product.Get;
    using MesJolisCotillons.Contracts.Responses.Product.Get;
    using MesJolisCotillons.Core;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Executors.Builders.Product.Get;
    using MesJolisCotillons.Response.Builders.Product.Get;
    using MesJolisCotillons.Validation.Builders.Product.Get;

    public class GetProductsOperation : IOperation<GetProductsRequest, GetProductsResponse>
    {
        private readonly IOperationBuilder<GetProductsRequest, GetProductsResponse, GetProductsCommand> operationBuilder;
        private readonly IGetProductsAdapter adapter;
        private readonly IGetProductsCommandBuilder commandBuilder;
        private readonly IGetProductsValidationBuilder validationBuilder;
        private readonly IGetProductsExecutorBuilder executorBuilder;
        private readonly IGetProductsResponseBuilder responseBuilder;

        public GetProductsOperation(
            IOperationBuilder<GetProductsRequest, GetProductsResponse, GetProductsCommand> operationBuilder,
            IGetProductsAdapter adapter,
            IGetProductsCommandBuilder commandBuilder,
            IGetProductsValidationBuilder validationBuilder,
            IGetProductsExecutorBuilder executorBuilder,
            IGetProductsResponseBuilder responseBuilder)
        {
            this.operationBuilder = operationBuilder;
            this.adapter = adapter;
            this.commandBuilder = commandBuilder;
            this.validationBuilder = validationBuilder;
            this.executorBuilder = executorBuilder;
            this.responseBuilder = responseBuilder;
        }

        public Task<GetProductsResponse> Run(GetProductsRequest request)
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
