namespace MesJolisCotillons.Core.Message.Builders
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts;
    using MesJolisCotillons.Resources.Services;

    public abstract class MessageBuilderBase<TCommand> : IMessageBuilder
        where TCommand : ICommand
    {
        protected readonly IMessagesLocalizerService messagesService;
        protected readonly MessageCode messageCode;

        public MessageBuilderBase(IMessagesLocalizerService messagesService, MessageCode messageCode)
        {
            this.messagesService = messagesService;
            this.messageCode = messageCode;
        }

        public MessageCode MessageCode => this.messageCode;

        public string GetMessageString(ICommand command)
        {
            var castedCommand = (TCommand)command;
            return this.GetMessageString(castedCommand);
        }

        public abstract string GetMessageString(TCommand command);
    }
}
