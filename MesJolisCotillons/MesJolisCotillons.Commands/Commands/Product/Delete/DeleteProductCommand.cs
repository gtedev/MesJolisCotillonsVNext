namespace MesJolisCotillons.Commands.Product.Delete
{
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public class DeleteProductCommand : ICommand,
    IExistingProductCommand
    {
        public DeleteProductCommand(int productId, ProductViewModel existingProduct)
        {
            this.ProductId = productId;
            this.ExistingProduct = existingProduct;
        }

        public int ProductId { get; }

        public ProductViewModel ExistingProduct { get; }
    }
}
