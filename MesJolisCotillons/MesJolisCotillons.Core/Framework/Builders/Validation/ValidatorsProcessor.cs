namespace MesJolisCotillons.Core.Framework.Builders.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts;
    using MesJolisCotillons.Core.Message.Builders;
    using MesJolisCotillons.Resources.Services;
    using MesJolisCotillons.Validation.Validators;

    public class ValidatorsProcessor<TCommand> : IValidatorsProcessor<TCommand>
        where TCommand : ICommand
    {
        private readonly IDictionary<MessageCode, IMessageBuilder> messageBuilders;
        private readonly IMessagesLocalizerService messageService;

        public ValidatorsProcessor(
            IEnumerable<IMessageBuilder> messageBuilders,
            IMessagesLocalizerService messageService)
        {
            this.messageBuilders = messageBuilders.ToDictionary(mb => mb.MessageCode, mb => mb);
            this.messageService = messageService;
        }

        public IValidationReport<TCommand> ProcessValidators(IEnumerable<IValidatorStep<TCommand>> validatorSteps, TCommand command)
        {
            var isValid = true;
            var validatedCommands = new List<ValidatedCommand<TCommand>>();
            foreach (var validatorStep in validatorSteps)
            {
                if (!validatorStep.CanContinueIfValid() && !isValid)
                {
                    return ValidationReport<TCommand>.CreateValidationReport(
                        isValid,
                        validatedCommands,
                        command);
                }

                var validator = validatorStep.GetValidator();
                if (validator == null)
                {
                    continue;
                }


                var isValidatorValid = validator.Validate(command);

                ValidatedCommand<TCommand> validatedCommand = this.BuildValidatedCommand(
                    isValidatorValid,
                    command,
                    validator);

                validatedCommands.Add(validatedCommand);

                // aggregate here all result from all validators in isValid variable
                isValid &= isValidatorValid;
            }

            return ValidationReport<TCommand>.CreateValidationReport(
                isValid,
                validatedCommands,
                command);
        }

        ValidatedCommand<TCommand> BuildValidatedCommand(bool isValid, TCommand command, IValidator<TCommand> validator)
        {
            if (isValid)
            {
                return this.BuildSuccessValidatedCommand(isValid, command, validator);
            }
            else
            {
                return this.BuildFailureValidatedCommand(isValid, command, validator);
            }
        }

        private ValidatedCommand<TCommand> BuildFailureValidatedCommand(bool isValid, TCommand command, IValidator<TCommand> validator)
        {
            var messageFailure = string.Empty;
            var messageBuilder = this.messageBuilders.ContainsKey(validator.MessageFailureCode)
                ? this.messageBuilders[validator.MessageFailureCode]
                : null;

            if (messageBuilder == null)
            {
                // no specific message builder, let's try to get an eventual message with just message code
                messageFailure = this.messageService.GetMessage(validator.MessageFailureCode);
                if (string.IsNullOrEmpty(messageFailure))
                {
                    throw new System.Exception($"ValidatorProcess: Message Builder not found for MessageCode {validator.MessageFailureCode.ToString()}," +
                        $" or not existing message code in Resource");
                }
            }
            else
            {
                messageFailure = messageBuilder.GetMessageString(command);
            }

            return new ValidatedCommand<TCommand>(
                isValid,
                command,
                validator.GetType().Name,
                messageFailure);
        }

        private ValidatedCommand<TCommand> BuildSuccessValidatedCommand(bool isValid, TCommand command, IValidator<TCommand> validator)
        {
            return new ValidatedCommand<TCommand>(
                isValid,
                command,
                validator.GetType().Name);
        }
    }
}