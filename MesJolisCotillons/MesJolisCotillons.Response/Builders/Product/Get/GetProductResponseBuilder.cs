namespace MesJolisCotillons.Response.Builders.Product.Get
{
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.Contracts.Responses.Product.Get;
    using MesJolisCotillons.Resources.Services;
    using MesJolisCotillons.Validation.Validators;

    public class GetProductResponseBuilder : ResponseBuilderBase, IGetProductResponseBuilder
    {
        public GetProductResponseBuilder(IMessagesLocalizerService messagesService)
            : base(messagesService)
        {
        }

        public GetProductResponse Build(GetProductResponse response, IValidationReport<GetProductCommand> validationReport)
        {
            response.Product = validationReport.Command.ExistingProduct;

            return response;
        }
    }
}
