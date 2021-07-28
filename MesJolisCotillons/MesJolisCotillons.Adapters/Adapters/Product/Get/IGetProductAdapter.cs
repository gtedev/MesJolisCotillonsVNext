namespace MesJolisCotillons.Adapters.Product.Get
{
    using MesJolisCotillons.Contracts.Requests.Product.Get;
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public interface IGetProductAdapter : IAdapter<GetProductRequest>
    {
        ProductViewModel ExistingProduct { get; }
    }
}
