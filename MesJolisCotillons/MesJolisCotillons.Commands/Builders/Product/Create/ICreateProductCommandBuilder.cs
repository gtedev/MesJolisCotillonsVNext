namespace MesJolisCotillons.Commands.Builders.Product.Create
{
    using MesJolisCotillons.Commands.Product.Create;
    using MesJolisCotillons.Contracts.Requests.Product.Create;

    public interface ICreateProductCommandBuilder : ICommandBuilder<CreateProductCommand, CreateProductRequest>
    {
    }
}
