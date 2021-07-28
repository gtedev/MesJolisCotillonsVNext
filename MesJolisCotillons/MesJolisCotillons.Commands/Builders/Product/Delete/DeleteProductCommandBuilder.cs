namespace MesJolisCotillons.Commands.Builders.Product.Delete
{
    using MesJolisCotillons.Adapters.Product.Delete;
    using MesJolisCotillons.Commands.Product.Delete;
    using MesJolisCotillons.Contracts.Requests.Product.Delete;

    public class DeleteProductCommandBuilder : IDeleteProductCommandBuilder
    {
        public DeleteProductCommand Build(IDeleteProductAdapter adapter, DeleteProductRequest request)
        {
            return new DeleteProductCommand(request.ProductId, adapter.ExistingProduct);
        }
    }
}
