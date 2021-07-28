namespace MesJolisCotillons.Commands.Builders.Product.Get
{
    using MesJolisCotillons.Adapters.Product.Get;
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.Contracts.Requests.Product.Get;

    public interface IGetProductsCommandBuilder : ICommandBuilder<GetProductsCommand, GetProductsRequest, IGetProductsAdapter>
    {
    }
}
