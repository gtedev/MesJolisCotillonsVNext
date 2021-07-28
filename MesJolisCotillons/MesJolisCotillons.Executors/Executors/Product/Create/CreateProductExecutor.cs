namespace MesJolisCotillons.Executors.Product.Create
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands.Product.Create;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;

    public class CreateProductExecutor : ICreateProductExecutor
    {
        private readonly IProductRepository productRepository;

        public CreateProductExecutor(IProductRepository productRepository)
            => this.productRepository = productRepository;

        public async Task ExecuteAsync(CreateProductCommand command)
        {
            var productViewResolver = await this.productRepository.CreateProduct(
             command.Name,
             command.Description,
             command.Price,
             command.DisplayName);

            // set the resolver that will be executed in ResponseBuilder
            // to retrieve product created infos.
            command.ProductViewResolver = productViewResolver;
        }
    }
}
