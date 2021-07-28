namespace MesJolisCotillons.Response.Builders.Product.Create
{
    using MesJolisCotillons.Commands.Product.Create;
    using MesJolisCotillons.Contracts.Responses.Product.Create;

    public interface ICreateProductResponseBuilder : IResponseBuilder<CreateProductCommand, CreateProductResponse>
    {
    }
}
