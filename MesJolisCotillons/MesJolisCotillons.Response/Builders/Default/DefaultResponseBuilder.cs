namespace MesJolisCotillons.Response.Builders.Default
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Resources.Services;
    using MesJolisCotillons.Validation.Validators;

    public class DefaultResponseBuilder<TCommand, TResponse> : ResponseBuilderBase, IDefaultResponseBuilder<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        public DefaultResponseBuilder(IMessagesLocalizerService messagesService)
            : base(messagesService)
        {
        }

        public TResponse Build(TResponse response, IValidationReport<TCommand> validationReport)
        {
            var message = this.messagesService
               .GetMessage(MessageCode.DefaultReponseSuccess, validationReport.OperationName);

            response.Messages.Add(message);
            return response;
        }
    }
}
