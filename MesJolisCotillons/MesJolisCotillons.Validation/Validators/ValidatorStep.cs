namespace MesJolisCotillons.Validation.Validators
{
    using System;
    using MesJolisCotillons.Commands;

    public class ValidatorStep<TCommand> : IValidatorStep<TCommand>
        where TCommand : ICommand
    {
        private IValidator<TCommand> validator;

        public ValidatorStep(Func<bool> canContinueIfValid, IValidator<TCommand> validator)
        {
            this.CanContinueIfValid = canContinueIfValid;
            this.validator = validator;
        }

        public Func<bool> CanContinueIfValid { get; }

        public IValidator<TCommand> GetValidator() => this.validator;
    }
}
