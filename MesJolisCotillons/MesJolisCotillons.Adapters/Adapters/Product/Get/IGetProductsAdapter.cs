namespace MesJolisCotillons.Adapters.Product.Get
{
    using System.Collections.Generic;
    using MesJolisCotillons.Contracts.Requests.Product.Get;
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public interface IGetProductsAdapter : IAdapter<GetProductsRequest>
    {
        IReadOnlyCollection<CategoryViewModel> ExistingCategories { get; set; }
    }
}
