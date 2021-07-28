namespace MesJolisCotillons.Executors.Product.Delete
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands.Product.Delete;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;

    public class DeleteProductExecutor : IDeleteProductExecutor
    {
        private readonly IProductRepository productRepository;

        public DeleteProductExecutor(IProductRepository productRepository)
            => this.productRepository = productRepository;

        public async Task ExecuteAsync(DeleteProductCommand command)
        {
            await this.productRepository.DeleteProduct(command.ProductId);
        }
    }
}
