namespace MesJolisCotillons.Adapters.Product.Get
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests.Product.Get;
    using MesJolisCotillons.Contracts.ViewModels.Product;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;

    public class GetProductsAdapter : AdapterBase<GetProductsRequest>, IGetProductsAdapter
    {
        private IProductRepository productRepository;

        public GetProductsAdapter(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public override async Task InitAdapter(GetProductsRequest request)
        {
            this.ExistingCategories = await this.productRepository.FindAllCategories();
        }

        public IReadOnlyCollection<CategoryViewModel> ExistingCategories { get; set; }
    }
}
