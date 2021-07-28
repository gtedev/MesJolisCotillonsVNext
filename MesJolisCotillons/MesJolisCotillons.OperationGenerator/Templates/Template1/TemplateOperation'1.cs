namespace MesJolisCotillons.Operations.TemplateNamespace
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Adapters.TemplateNamespace;
    using MesJolisCotillons.Commands.Builders.TemplateNamespace;
    using MesJolisCotillons.Commands.TemplateNamespace;
    using MesJolisCotillons.Contracts.Requests.TemplateNamespace;
    using MesJolisCotillons.Contracts.Responses.TemplateNamespace;
    using MesJolisCotillons.Core;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Executors.Builders.TemplateNamespace;
    using MesJolisCotillons.Response.Builders.TemplateNamespace;
    using MesJolisCotillons.Validation.Builders.TemplateNamespace;

    public class TemplateOperationNameOperation : IOperation<TemplateOperationNameRequest, TemplateOperationNameResponse>
    {
        private readonly IOperationBuilder<TemplateOperationNameRequest, TemplateOperationNameResponse, TemplateOperationNameCommand> operationBuilder;
        private readonly ITemplateOperationNameAdapter adapter;
        private readonly ITemplateOperationNameCommandBuilder commandBuilder;
        private readonly ITemplateOperationNameValidationBuilder validationBuilder;
        private readonly ITemplateOperationNameExecutorBuilder executorBuilder;
        private readonly ITemplateOperationNameResponseBuilder responseBuilder;

        public TemplateOperationNameOperation(
            IOperationBuilder<TemplateOperationNameRequest, TemplateOperationNameResponse, TemplateOperationNameCommand> operationBuilder,
            ITemplateOperationNameAdapter adapter,
            ITemplateOperationNameCommandBuilder commandBuilder,
            ITemplateOperationNameValidationBuilder validationBuilder,
            ITemplateOperationNameExecutorBuilder executorBuilder,
            ITemplateOperationNameResponseBuilder responseBuilder)
        {
            this.operationBuilder = operationBuilder;
            this.adapter = adapter;
            this.commandBuilder = commandBuilder;
            this.validationBuilder = validationBuilder;
            this.executorBuilder = executorBuilder;
            this.responseBuilder = responseBuilder;
        }

        public Task<TemplateOperationNameResponse> Run(TemplateOperationNameRequest request)
        {
            return this.operationBuilder
                    .AddAdapter(this.adapter, request)
                    .AddCommandBuilder(this.commandBuilder)
                    .AddValidationBuilder(this.validationBuilder)
                    .AddExecutorBuilder(this.executorBuilder)
                    .AddResponseBuilder(this.responseBuilder)
                    .Build()
                    .Run();
        }
    }
}
