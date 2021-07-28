namespace MesJolisCotillons.Commands.Builders.TemplateNamespace
{
    using MesJolisCotillons.Adapters.TemplateNamespace;
    using MesJolisCotillons.Commands.TemplateNamespace;
    using MesJolisCotillons.Contracts.Requests.TemplateNamespace;

    public class TemplateOperationNameCommandBuilder : ITemplateOperationNameCommandBuilder
    {
        public TemplateOperationNameCommand Build(ITemplateOperationNameAdapter adapter, TemplateOperationNameRequest request)
        {
            return new TemplateOperationNameCommand
            {
            };
        }
    }
}
