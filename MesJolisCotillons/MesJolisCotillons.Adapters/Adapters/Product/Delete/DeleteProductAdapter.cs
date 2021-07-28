namespace MesJolisCotillons.Adapters.Product.Delete
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests.Product.Delete;
    using MesJolisCotillons.Contracts.ViewModels.Product;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;

    public class DeleteProductAdapter : AdapterBase<DeleteProductRequest>, IDeleteProductAdapter
    {
        private IProductRepository productRepository;

        public DeleteProductAdapter(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public override async Task InitAdapter(DeleteProductRequest request)
        {
            this.ExistingProduct = await this.productRepository.FindProduct(request.ProductId);
        }

        public ProductViewModel ExistingProduct { get; set; }
    }
}
