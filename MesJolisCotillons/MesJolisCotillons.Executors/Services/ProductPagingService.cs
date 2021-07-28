namespace MesJolisCotillons.Executors.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public class ProductPagingService : IProductPagingService
    {
        public IReadOnlyCollection<ProductViewModel> GetPagedProducts(
            IReadOnlyCollection<ProductViewModel> products,
            int page,
            int pageSize)
        {
            var start = (page - 1) * pageSize;
            var productPaged = products
                .Skip(start)
                .Take(pageSize)
                .ToList();

            return productPaged;
        }
    }
}
