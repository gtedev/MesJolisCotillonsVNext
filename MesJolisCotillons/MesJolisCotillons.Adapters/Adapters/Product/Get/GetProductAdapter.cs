namespace MesJolisCotillons.Adapters.Product.Get
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests.Product.Get;
    using MesJolisCotillons.Contracts.ViewModels.Product;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;

    public class GetProductAdapter : AdapterBase<GetProductRequest>, IGetProductAdapter
    {
        private IProductRepository productRepository;

        public GetProductAdapter(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public override async Task InitAdapter(GetProductRequest request)
        {
            this.ExistingProduct = await this.productRepository.FindProduct(request.ProductId, includePictures: true);
        }

        public ProductViewModel ExistingProduct { get; set; }
    }
}
