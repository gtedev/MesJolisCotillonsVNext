namespace MesJolisCotillons.Commands.Builders.Product.Get
{
    using MesJolisCotillons.Adapters.Product.Get;
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.Contracts.Requests.Product.Get;

    public class GetProductCommandBuilder : IGetProductCommandBuilder
    {
        public GetProductCommand Build(IGetProductAdapter adapter, GetProductRequest request)
        {
            return new GetProductCommand(request.ProductId, adapter.ExistingProduct);
        }
    }
}
