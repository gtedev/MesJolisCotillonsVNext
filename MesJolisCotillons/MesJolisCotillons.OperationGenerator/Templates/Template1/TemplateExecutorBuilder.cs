namespace MesJolisCotillons.Executors.Builders.TemplateNamespace
{
    using MesJolisCotillons.Commands.TemplateNamespace;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Executors.Builders.TemplateNamespace;
    using System.Collections.Generic;

    public class TemplateOperationNameExecutorBuilder : ExecutorBuilderBase<TemplateOperationNameCommand>, ITemplateOperationNameExecutorBuilder
    {
        public TemplateOperationNameExecutorBuilder(
            IExecutorStepsBuilder<TemplateOperationNameCommand> builder)
            : base(builder)
        {
        }

        public override IEnumerable<IExecutorStep<TemplateOperationNameCommand>> Build(TemplateOperationNameCommand command)
        {
        }
    }
}
