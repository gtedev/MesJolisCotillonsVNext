namespace MesJolisCotillons.Response.Builders
{
    using MesJolisCotillons.Resources.Services;

    public abstract class ResponseBuilderBase
    {
        protected readonly IMessagesLocalizerService messagesService;

        public ResponseBuilderBase(IMessagesLocalizerService messagesService)
            => this.messagesService = messagesService;
    }
}
