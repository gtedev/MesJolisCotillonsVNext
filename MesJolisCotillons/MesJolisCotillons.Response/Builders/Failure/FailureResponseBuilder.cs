namespace MesJolisCotillons.Response.Builders.Default
{
    using System.Linq;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Resources.Services;
    using MesJolisCotillons.Response.Builders.Failure;
    using MesJolisCotillons.Validation.Validators;

    public class FailureResponseBuilder<TCommand, TResponse> : ResponseBuilderBase, IFailureResponseBuilder<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        public FailureResponseBuilder(IMessagesLocalizerService messagesService)
            : base(messagesService)
        {
        }

        public TResponse Build(TResponse response, IValidationReport<TCommand> validationReport)
        {
            var messages = validationReport
                .ValidatedCommands
                .Where(vc => !vc.IsValid)
                .Select(v => v.FailureMessage);

            response.Messages.AddRange(messages);
            return response;
        }
    }
}
