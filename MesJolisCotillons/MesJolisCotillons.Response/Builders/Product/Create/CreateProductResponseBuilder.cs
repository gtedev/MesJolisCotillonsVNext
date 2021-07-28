namespace MesJolisCotillons.Response.Builders.Product.Create
{
    using MesJolisCotillons.Commands.Product.Create;
    using MesJolisCotillons.Contracts;
    using MesJolisCotillons.Contracts.Responses.Product.Create;
    using MesJolisCotillons.Resources.Services;
    using MesJolisCotillons.Validation.Validators;

    public class CreateProductResponseBuilder : ResponseBuilderBase, ICreateProductResponseBuilder
    {
        public CreateProductResponseBuilder(IMessagesLocalizerService messagesService)
            : base(messagesService)
        {
        }

        public CreateProductResponse Build(CreateProductResponse response, IValidationReport<CreateProductCommand> validationReport)
        {
            var createdProductView = validationReport.Command.ProductViewResolver();

            var message = this.messagesService
                .GetMessage(MessageCode.CreateProductSuccess, validationReport.Command.Name);

            response.Messages.Add(message);
            response.Product = createdProductView;

            return response;
        }
    }
}
