namespace MesJolisCotillons.Commands.Builders.TemplateNamespace
{
    using MesJolisCotillons.Commands.TemplateNamespace;
    using MesJolisCotillons.Contracts.Requests.TemplateNamespace;

    public class TemplateOperationNameCommandBuilder : ITemplateOperationNameCommandBuilder
    {
        public TemplateOperationNameCommand Build(TemplateOperationNameRequest request)
        {
            return new TemplateOperationNameCommand
            {
                ////Name = request.Name,
            };
        }
    }
}
