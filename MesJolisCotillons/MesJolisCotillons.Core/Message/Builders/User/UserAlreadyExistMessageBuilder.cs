namespace MesJolisCotillons.Core.Message.Builders.User
{
    using MesJolisCotillons.Commands.Commands.User;
    using MesJolisCotillons.Resources.Services;

    public class UserAlreadyExistMessageBuilder : MessageBuilderBase<IExistingUserCommand>, IMessageBuilder
    {
        public UserAlreadyExistMessageBuilder(IMessagesLocalizerService messagesService)
            : base(messagesService, Contracts.MessageCode.UserAlreadyExists)
        {
        }

        public override string GetMessageString(IExistingUserCommand command)
        {
            return this.messagesService
                .GetMessage(this.MessageCode, command.ExistingUser.Email);
        }
    }
}
