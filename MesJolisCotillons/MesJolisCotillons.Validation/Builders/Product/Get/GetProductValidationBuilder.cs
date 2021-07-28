namespace MesJolisCotillons.Validation.Builders.Product.Get
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.Validation.Validators;
    using MesJolisCotillons.Validation.Validators.Product;

    public class GetProductValidationBuilder : ValidationBuilderBase<GetProductCommand>, IGetProductValidationBuilder
    {
        public GetProductValidationBuilder(IValidationStepsBuilder<GetProductCommand> builder)
            : base(builder)
        {
        }

        public override IEnumerable<IValidatorStep<GetProductCommand>> Build()
        {
            return this.Builder
                .AddValidator<ProductExists>()
                .AddBreakIfNoValidStep()
                .Build();
        }
    }
}
