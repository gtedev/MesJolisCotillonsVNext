namespace MesJolisCotillons.Core.Message.Builders.Product
{
    using MesJolisCotillons.Commands.Product;
    using MesJolisCotillons.Resources.Services;

    public class ProductDoesNotExistMessageBuilder : MessageBuilderBase<IExistingProductCommand>, IMessageBuilder
    {
        public ProductDoesNotExistMessageBuilder(IMessagesLocalizerService messagesService)
            : base(messagesService, Contracts.MessageCode.ProductDoesNotExist)
        {
        }

        public override string GetMessageString(IExistingProductCommand command)
        {
            return this.messagesService
                .GetMessage(this.MessageCode, command.ProductId.ToString());
        }
    }
}
