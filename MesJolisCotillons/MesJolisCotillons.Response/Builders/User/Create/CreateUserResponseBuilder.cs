namespace MesJolisCotillons.Response.Builders.User.Create
{
    using MesJolisCotillons.Commands.User.Create;
    using MesJolisCotillons.Contracts;
    using MesJolisCotillons.Contracts.Responses.User.Create;
    using MesJolisCotillons.Resources.Services;
    using MesJolisCotillons.Validation.Validators;

    public class CreateUserResponseBuilder : ResponseBuilderBase, ICreateUserResponseBuilder
    {
        public CreateUserResponseBuilder(IMessagesLocalizerService messagesService)
            : base(messagesService)
        {
        }

        public CreateUserResponse Build(CreateUserResponse response, IValidationReport<CreateUserCommand> validationReport)
        {
            var message = this.messagesService
                .GetMessage(MessageCode.DefaultReponseSuccess, validationReport.Command.Email);

            response.Messages.Add(message);
            return response;
        }
    }
}
