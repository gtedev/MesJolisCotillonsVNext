namespace MesJolisCotillons.Response.Builders.Product.Delete
{
    using MesJolisCotillons.Commands.Product.Delete;
    using MesJolisCotillons.Contracts;
    using MesJolisCotillons.Contracts.Responses.Product.Delete;
    using MesJolisCotillons.Resources.Services;
    using MesJolisCotillons.Validation.Validators;

    public class DeleteProductResponseBuilder : ResponseBuilderBase, IDeleteProductResponseBuilder
    {
        public DeleteProductResponseBuilder(IMessagesLocalizerService messagesService)
            : base(messagesService)
        {
        }

        public DeleteProductResponse Build(DeleteProductResponse response, IValidationReport<DeleteProductCommand> validationReport)
        {
            var message = this.messagesService
                .GetMessage(MessageCode.DeleteProductSuccess, validationReport.Command.ProductId.ToString());

            response.Messages.Add(message);
            return response;
        }
    }
}
