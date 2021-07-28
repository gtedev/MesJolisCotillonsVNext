namespace MesJolisCotillons.Executors.Product.Get
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;
    using MesJolisCotillons.Executors.Services;

    public class GetProductsExecutor : IGetProductsExecutor
    {
        private readonly IProductRepository productRepository;
        private readonly IProductPagingService productPagingService;

        public GetProductsExecutor(
            IProductRepository productRepository,
            IProductPagingService productPagingService)
        {
            this.productRepository = productRepository;
            this.productPagingService = productPagingService;
        }

        public async Task ExecuteAsync(GetProductsCommand command)
        {
            var products = await this.productRepository
                .FindAllProducts(
                command.IncludeFirstPicture,
                command.ProductCategories);

            var productPaged = this.productPagingService.GetPagedProducts(
                products,
                command.Page,
                command.PageSize);

            command.ProductsResult = productPaged;
        }
    }
}
