using MesJolisCotillons.Commands;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Validation.Validators;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace MesJolisCotillons.Core.UnitTests.Helpers
{
    public class ValidationStepsBuilderHelper<TCommand> 
        where TCommand : ICommand
    {
        private List<IValidatorStep<TCommand>> validationStepsMocks = new List<IValidatorStep<TCommand>>();

        public ValidationStepsBuilderHelper<TCommand> AddValidationStep(
            TCommand command,
            bool isValidatorValid = false,
            bool canContinueIfValid = true,
            MessageCode messageFailureCode = MessageCode.DefaultReponseSuccess)
        {
            var validator = Substitute.For<IValidator<TCommand>>();
            validator.Validate(command).Returns(isValidatorValid);
            validator.MessageFailureCode.Returns(messageFailureCode);

            var validatorStep = Substitute.For<IValidatorStep<TCommand>>();
            validatorStep.GetValidator().Returns(validator);
            validatorStep.CanContinueIfValid().Returns(canContinueIfValid);

            this.validationStepsMocks.Add(validatorStep);

            return this;
        }

        public static ValidationStepsBuilderHelper<TCommand> CreateValidationStepsBuilderHelper()
            => new ValidationStepsBuilderHelper<TCommand>();

        public IEnumerable<IValidatorStep<TCommand>> Build()
            => this.validationStepsMocks;
    }
}
