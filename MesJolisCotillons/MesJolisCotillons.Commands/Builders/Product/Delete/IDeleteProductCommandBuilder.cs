namespace MesJolisCotillons.Commands.Builders.Product.Delete
{
    using MesJolisCotillons.Adapters.Product.Delete;
    using MesJolisCotillons.Commands.Product.Delete;
    using MesJolisCotillons.Contracts.Requests.Product.Delete;

    public interface IDeleteProductCommandBuilder : ICommandBuilder<DeleteProductCommand, DeleteProductRequest, IDeleteProductAdapter>
    {
    }
}
