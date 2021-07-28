namespace MesJolisCotillons.Validation.Validators
{
    using System;
    using MesJolisCotillons.Commands;

    public interface IValidatorStep<in TCommand>
        where TCommand : ICommand
    {
        IValidator<TCommand> GetValidator();

        Func<bool> CanContinueIfValid { get; }
    }
}
