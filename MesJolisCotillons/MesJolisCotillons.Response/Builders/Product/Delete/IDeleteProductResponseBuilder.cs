namespace MesJolisCotillons.Response.Builders.Product.Delete
{
    using MesJolisCotillons.Commands.Product.Delete;
    using MesJolisCotillons.Contracts.Responses.Product.Delete;

    public interface IDeleteProductResponseBuilder : IResponseBuilder<DeleteProductCommand, DeleteProductResponse>
    {
    }
}
