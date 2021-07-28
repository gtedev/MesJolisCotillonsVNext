namespace MesJolisCotillons.Commands.Product
{
    public interface IProductsPagedCommand : ICommand
    {
        int Page { get; }

        int PageSize { get; }
    }
}
