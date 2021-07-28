namespace MesJolisCotillons.Validation.Validators
{
    using MesJolisCotillons.Commands;

    public class ValidatedCommand<TCommand>
        where TCommand : ICommand
    {
        public string ValidationStepName { get; }

        public string FailureMessage { get; }

        public bool IsValid { get; }

        public TCommand Command { get; }

        public ValidatedCommand(bool isValid, TCommand command, string validationStepName)
        {
            this.IsValid = isValid;
            this.Command = command;
            this.ValidationStepName = validationStepName;
        }

        public ValidatedCommand(bool isValid, TCommand command, string validationStepName, string failureMessage)
        {
            this.IsValid = isValid;
            this.Command = command;
            this.ValidationStepName = validationStepName;
            this.FailureMessage = failureMessage;
        }
    }
}
