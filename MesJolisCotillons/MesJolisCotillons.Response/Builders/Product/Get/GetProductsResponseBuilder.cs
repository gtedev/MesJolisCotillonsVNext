namespace MesJolisCotillons.Response.Builders.Product.Get
{
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.Contracts;
    using MesJolisCotillons.Contracts.Responses.Product.Get;
    using MesJolisCotillons.Resources.Services;
    using MesJolisCotillons.Validation.Validators;

    public class GetProductsResponseBuilder : IGetProductsResponseBuilder
    {
        private readonly IMessagesLocalizerService messagesService;

        public GetProductsResponseBuilder(IMessagesLocalizerService messagesService)
        => this.messagesService = messagesService;

        public GetProductsResponse Build(GetProductsResponse response, IValidationReport<GetProductsCommand> validationReport)
        {
            response.Products = validationReport.Command.ProductsResult;

            return response;
        }
    }
}
