namespace MesJolisCotillons.Resources.Services
{
    using System.Linq;
    using MesJolisCotillons.Contracts;

    public class MessagesLocalizerService : IMessagesLocalizerService
    {
        private readonly IResourceLocalizerService resourceLocalizerService;

        public MessagesLocalizerService(IResourceLocalizerService resourceLocalizerService)
            => this.resourceLocalizerService = resourceLocalizerService;

        public string GetMessage(MessageCode messageCode, params string[] arguments)
        {
            var message = this.resourceLocalizerService
                .GetResourceValue(messageCode.ToString(), ResourceName.Messages);

            if (!arguments.Any())
            {
                return message;
            }

            return string.Format(message, arguments);
        }
    }
}
