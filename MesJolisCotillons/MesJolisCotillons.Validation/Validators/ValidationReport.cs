namespace MesJolisCotillons.Validation.Validators
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands;

    public class ValidationReport<TCommand> : IValidationReport<TCommand>
        where TCommand : ICommand
    {
        public ValidationReport(
            TCommand command,
            bool isValid,
            IEnumerable<ValidatedCommand<TCommand>> validatedCommands)
        {
            Command = command;
            IsValid = isValid;
            ValidatedCommands = validatedCommands;
            OperationName = GetOperationName();
        }

        public TCommand Command { get; }

        public string OperationName { get; }

        public bool IsValid { get; }

        public IEnumerable<ValidatedCommand<TCommand>> ValidatedCommands { get; set; }

        public static IValidationReport<TCommand> CreateDefaultValidationReport(TCommand command)
        {
            return new ValidationReport<TCommand>
            (
                command,
                true,
                new List<ValidatedCommand<TCommand>>()
            );
        }

        public static IValidationReport<TCommand> CreateValidationReport(
            bool isValid,
            IEnumerable<ValidatedCommand<TCommand>> validatedCommands,
            TCommand command)
        {
            return new ValidationReport<TCommand>
            (
                command,
                isValid,
                validatedCommands
            );
        }

        private static string GetOperationName()
        {
            return typeof(TCommand).Name.Replace("Command", string.Empty);
        }
    }
}
