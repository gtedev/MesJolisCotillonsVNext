namespace MesJolisCotillons.Executors.Services
{
    using System.Collections.Generic;
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public interface IProductPagingService
    {
        IReadOnlyCollection<ProductViewModel> GetPagedProducts(IReadOnlyCollection<ProductViewModel> products, int page, int pageSize);
    }
}
