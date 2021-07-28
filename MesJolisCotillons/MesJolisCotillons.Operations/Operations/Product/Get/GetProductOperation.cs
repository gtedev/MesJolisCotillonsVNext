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
    using MesJolisCotillons.Response.Builders.Product.Get;
    using MesJolisCotillons.Validation.Builders.Product.Get;

    public class GetProductOperation : IOperation<GetProductRequest, GetProductResponse>
    {
        private readonly IOperationBuilder<GetProductRequest, GetProductResponse, GetProductCommand> operationBuilder;
        private readonly IGetProductAdapter adapter;
        private readonly IGetProductCommandBuilder commandBuilder;
        private readonly IGetProductValidationBuilder validationBuilder;
        private readonly IGetProductResponseBuilder responseBuilder;

        public GetProductOperation(
            IOperationBuilder<GetProductRequest, GetProductResponse, GetProductCommand> operationBuilder,
            IGetProductAdapter adapter,
            IGetProductCommandBuilder commandBuilder,
            IGetProductValidationBuilder validationBuilder,
            IGetProductResponseBuilder responseBuilder)
        {
            this.operationBuilder = operationBuilder;
            this.adapter = adapter;
            this.commandBuilder = commandBuilder;
            this.validationBuilder = validationBuilder;
            this.responseBuilder = responseBuilder;
        }

        public Task<GetProductResponse> Run(GetProductRequest request)
        {
            return this.operationBuilder
                    .AddAdapter(this.adapter, request)
                    .AddCommandBuilder(this.commandBuilder)
                    .AddValidationBuilder(this.validationBuilder)
                    .AddResponseBuilder(this.responseBuilder)
                    .Build()
                    .Run();
        }
    }
}
