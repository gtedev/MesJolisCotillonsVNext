namespace MesJolisCotillons.Core.Message.Builders.User
{
    using MesJolisCotillons.Commands.Commands.User;
    using MesJolisCotillons.Resources.Services;

    public class PasswordIsNotLongEnoughMessageBuilder : MessageBuilderBase<IUserPasswordCommand>, IMessageBuilder
    {
        public PasswordIsNotLongEnoughMessageBuilder(IMessagesLocalizerService messagesService)
            : base(messagesService, Contracts.MessageCode.PasswordIsNotLongEnough)
        {
        }

        public override string GetMessageString(IUserPasswordCommand command)
        {
            return this.messagesService
                .GetMessage(this.MessageCode, command.MinimumPasswordCharacterNumber.ToString());
        }
    }
}
