namespace MesJolisCotillons.Validation.Builders.TemplateNamespace
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands.TemplateNamespace;
    using MesJolisCotillons.Validation.Validators;

    public class TemplateOperationNameValidationBuilder : ValidationBuilderBase<TemplateOperationNameCommand>, ITemplateOperationNameValidationBuilder
    {
        public TemplateOperationNameValidationBuilder(IValidationStepsBuilder<TemplateOperationNameCommand> builder)
            : base(builder)
        {
        }

        public override IEnumerable<IValidatorStep<TemplateOperationNameCommand>> Build()
        {
        }
    }
}
