﻿namespace MesJolisCotillons.Commands.Builders.TemplateNamespace
{
    using MesJolisCotillons.Adapters.TemplateNamespace;
    using MesJolisCotillons.Commands.TemplateNamespace;
    using MesJolisCotillons.Contracts.Requests.TemplateNamespace;

    public interface ITemplateOperationNameCommandBuilder : ICommandBuilder<TemplateOperationNameCommand, TemplateOperationNameRequest, ITemplateOperationNameAdapter>
    {
    }
}