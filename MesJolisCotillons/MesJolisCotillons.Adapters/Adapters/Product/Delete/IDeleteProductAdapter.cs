namespace MesJolisCotillons.Adapters.Product.Delete
{
    using MesJolisCotillons.Contracts.Requests.Product.Delete;
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public interface IDeleteProductAdapter : IAdapter<DeleteProductRequest>
    {
        ProductViewModel ExistingProduct { get; }
    }
}
