namespace MesJolisCotillons.Validation.Builders.Product.Delete
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands.Product.Delete;
    using MesJolisCotillons.Validation.Validators;
    using MesJolisCotillons.Validation.Validators.Product;

    public class DeleteProductValidationBuilder : ValidationBuilderBase<DeleteProductCommand>, IDeleteProductValidationBuilder
    {
        public DeleteProductValidationBuilder(IValidationStepsBuilder<DeleteProductCommand> builder)
            : base(builder)
        {
        }

        public override IEnumerable<IValidatorStep<DeleteProductCommand>> Build()
        {
            return this.Builder
                .AddValidator<ProductExists>()
                .AddBreakIfNoValidStep()
                .Build();
        }
    }
}
