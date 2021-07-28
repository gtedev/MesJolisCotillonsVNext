namespace MesJolisCotillons.Response.Builders.TemplateNamespace
{
    using MesJolisCotillons.Commands.TemplateNamespace;
    using MesJolisCotillons.Contracts;
    using MesJolisCotillons.Contracts.Responses.TemplateNamespace;
    using MesJolisCotillons.Resources.Services;
    using MesJolisCotillons.Validation.Validators;

    public class TemplateOperationNameResponseBuilder : ITemplateOperationNameResponseBuilder
    {
        private readonly IMessagesLocalizerService messagesService;

        public TemplateOperationNameResponseBuilder(IMessagesLocalizerService messagesService)
        => this.messagesService = messagesService;

        public TemplateOperationNameResponse Build(TemplateOperationNameResponse response, IValidationReport<TemplateOperationNameCommand> validationReport)
        {
        }
    }
}
