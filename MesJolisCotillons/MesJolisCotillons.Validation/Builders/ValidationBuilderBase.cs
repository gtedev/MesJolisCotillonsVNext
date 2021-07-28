namespace MesJolisCotillons.Validation.Builders
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Validation.Validators;
    using System.Collections.Generic;

    public abstract class ValidationBuilderBase<TCommand> : IValidationBuilder<TCommand>
        where TCommand : ICommand
    {
        protected readonly IValidationStepsBuilder<TCommand> Builder;

        public ValidationBuilderBase(IValidationStepsBuilder<TCommand> builder)
        {
            this.Builder = builder;
        }

        IEnumerable<IValidatorStep<TCommand>> IValidationBuilder<TCommand>.Build()
        {
            return this.Build();
        }

        public abstract IEnumerable<IValidatorStep<TCommand>> Build();
    }
}
