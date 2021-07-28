namespace MesJolisCotillons.Validation.Validators
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands;

    public interface IValidationReport<TCommand>
        where TCommand : ICommand
    {
        bool IsValid { get; }

        TCommand Command { get; }

        IEnumerable<ValidatedCommand<TCommand>> ValidatedCommands { get; }

        string OperationName { get; }
    }
}
