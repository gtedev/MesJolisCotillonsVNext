namespace MesJolisCotillons.Validation.Builders
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Validation.Validators;
    using System.Collections.Generic;

    public class ValidationStepsBuilder<TCommand> : IValidationStepsBuilder<TCommand>
        where TCommand : ICommand
    {
        private List<IValidatorStep<TCommand>> validatorSteps = new List<IValidatorStep<TCommand>>();

        public IValidationStepsBuilder<TCommand> AddValidator<TValidator>()
            where TValidator : IValidator<TCommand>, new()
        {
            var validator = new TValidator();
            var validatorStep = new ValidatorStep<TCommand>(() => true, validator);
            this.validatorSteps.Add(validatorStep);

            return this;
        }

        public IValidationStepsBuilder<TCommand> AddBreakIfNoValidStep()
        {
            var validatorStep = new ValidatorStep<TCommand>(() => false, null);
            this.validatorSteps.Add(validatorStep);

            return this;
        }

        public IEnumerable<IValidatorStep<TCommand>> Build()
        {
            return this.validatorSteps;
        }
    }
}
