namespace MesJolisCotillons.Response.Builders.Product.Get
{
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.Contracts.Responses.Product.Get;

    public interface IGetProductResponseBuilder : IResponseBuilder<GetProductCommand, GetProductResponse>
    {
    }
}
