namespace MesJolisCotillons.Validation.Builders.Product.Get
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.Validation.Validators;
    using MesJolisCotillons.Validation.Validators.Product;

    public class GetProductsValidationBuilder : ValidationBuilderBase<GetProductsCommand>, IGetProductsValidationBuilder
    {
        public GetProductsValidationBuilder(IValidationStepsBuilder<GetProductsCommand> builder)
            : base(builder)
        {
        }

        public override IEnumerable<IValidatorStep<GetProductsCommand>> Build()
        {
            return this.Builder
                 .AddValidator<PageParametersGreaterThanOne>()
                 .AddValidator<CategoriesExist>()
                 .AddBreakIfNoValidStep()
                 .Build();
        }
    }
}
