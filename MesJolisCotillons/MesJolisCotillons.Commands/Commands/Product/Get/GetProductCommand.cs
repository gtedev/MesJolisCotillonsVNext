using MesJolisCotillons.Contracts.ViewModels.Product;

namespace MesJolisCotillons.Commands.Product.Get
{
    public class GetProductCommand : ICommand,
        IExistingProductCommand
    {
        public GetProductCommand(int productId, ProductViewModel existingProduct)
        {
            this.ProductId = productId;
            this.ExistingProduct = existingProduct;
        }

        public int ProductId { get; }

        public ProductViewModel ExistingProduct { get; }
    }
}
