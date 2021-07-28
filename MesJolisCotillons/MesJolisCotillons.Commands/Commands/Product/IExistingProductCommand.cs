namespace MesJolisCotillons.Commands.Product
{
    using MesJolisCotillons.Contracts.ViewModels.Product;

    public interface IExistingProductCommand : ICommand
    {
        int ProductId { get; }

        ProductViewModel ExistingProduct { get; }
    }
}
