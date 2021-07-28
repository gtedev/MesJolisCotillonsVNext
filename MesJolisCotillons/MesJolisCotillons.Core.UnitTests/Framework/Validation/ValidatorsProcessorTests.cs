using FluentAssertions;
using MesJolisCotillons.Common.UnitTests.Fake;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Core.Framework.Builders.Validation;
using MesJolisCotillons.Core.Message.Builders;
using MesJolisCotillons.Core.UnitTests.Helpers;
using MesJolisCotillons.Resources.Services;
using MesJolisCotillons.Validation.Validators;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Core.UnitTests.Framework.Validation
{
    public class ValidatorsProcessorTests
    {
        public abstract class ValidatorsProcessorTest
        {
            #region Mocks properties
            protected FakeCommand CommandMock;

            protected IEnumerable<IMessageBuilder> MessageBuildersMock;

            protected IMessagesLocalizerService MessageServiceMock;

            protected IEnumerable<IValidatorStep<FakeCommand>> ValidatorStepsMock;
            #endregion

            protected ValidatorsProcessor<FakeCommand> Processor { get; set; }

            public ValidatorsProcessorTest()
            {
                this.CommandMock = new FakeCommand();

                this.Arrange_Two_MessageBuilders();

                this.Processor = new ValidatorsProcessor<FakeCommand>(
                    this.MessageBuildersMock,
                    this.MessageServiceMock);
            }

            private void Arrange_Two_MessageBuilders()
            {
                var messageBuilder1 = Substitute.For<IMessageBuilder>();
                messageBuilder1.MessageCode.Returns(MessageCode.UserAlreadyExists);
                messageBuilder1.GetMessageString(this.CommandMock).Returns("messagebuilder UserAlreadyExists fake message");

                var messageBuilder2 = Substitute.For<IMessageBuilder>();
                messageBuilder2.MessageCode.Returns(MessageCode.DefaultReponseSuccess);
                messageBuilder2.GetMessageString(this.CommandMock).Returns("messagebuilder DefaultReponseSuccess fake message");

                this.MessageBuildersMock = new List<IMessageBuilder> { messageBuilder1, messageBuilder2 };
            }
        }

        public class ProcessValidatorsShould : ValidatorsProcessorTest
        {
            [Fact]
            public void Returns_Validation_True_When_Validators_Succeeded()
            {
                // Arrange
                this.Arrange_Three_Validators_That_Succeeds();

                // Act
                var reportResult = this.Processor.ProcessValidators(
                    this.ValidatorStepsMock,
                    this.CommandMock
                    );

                // Assert
                reportResult.IsValid.Should().BeTrue();
            }

            [Fact]
            public void Returns_Expected_ValidatedCommands_Count_When_Validators_Succeeded()
            {
                // Arrange
                this.Arrange_Three_Validators_That_Succeeds();

                // Act
                var reportResult = this.Processor.ProcessValidators(
                    this.ValidatorStepsMock,
                    this.CommandMock
                    );

                // Assert
                reportResult.ValidatedCommands.Should().HaveCount(3);
            }

            [Fact]
            public void Returns_Expected_ValidatedCommands_Properties_When_Validators_Succeeded()
            {
                // Arrange
                this.Arrange_Three_Validators_That_Succeeds();

                // Act
                var reportResult = this.Processor.ProcessValidators(
                    this.ValidatorStepsMock,
                    this.CommandMock
                    );

                // Assert
                var validatedCommands = reportResult.ValidatedCommands.ToList();
                validatedCommands[0].IsValid.Should().BeTrue();
                validatedCommands[0].FailureMessage.Should().BeNull();

                validatedCommands[1].IsValid.Should().BeTrue();
                validatedCommands[1].FailureMessage.Should().BeNull();

                validatedCommands[2].IsValid.Should().BeTrue();
                validatedCommands[2].FailureMessage.Should().BeNull();
            }

            [Fact]
            public void Returns_Validation_False_When_OneValidator_Failed()
            {
                // Arrange
                this.Arrange_Three_Validators_With_One_Fails();

                // Act
                var reportResult = this.Processor.ProcessValidators(
                    this.ValidatorStepsMock,
                    this.CommandMock
                    );

                // Assert
                reportResult.IsValid.Should().BeFalse();
            }

            [Fact]
            public void Returns_Expected_ValidatedCommands_Count_When_OneValidator_Failed_Without_BreakStep()
            {
                // Arrange
                this.Arrange_Three_Validators_With_One_Fails();

                // Act
                var reportResult = this.Processor.ProcessValidators(
                    this.ValidatorStepsMock,
                    this.CommandMock
                    );

                // Assert
                reportResult.ValidatedCommands.Should().HaveCount(3);
            }

            [Fact]
            public void Returns_Expected_ValidatedCommands_Properties_When_OneValidator_Failed_Without_BreakStep()
            {
                // Arrange
                this.Arrange_Three_Validators_With_One_Fails();

                // Act
                var reportResult = this.Processor.ProcessValidators(
                    this.ValidatorStepsMock,
                    this.CommandMock
                    );

                // Assert
                var validatedCommands = reportResult.ValidatedCommands.ToList();
                validatedCommands[0].IsValid.Should().BeTrue();
                validatedCommands[0].FailureMessage.Should().BeNull();

                validatedCommands[1].IsValid.Should().BeTrue();
                validatedCommands[1].FailureMessage.Should().BeNull();

                validatedCommands[2].IsValid.Should().BeFalse();
                validatedCommands[2].FailureMessage.Should().Be("messagebuilder UserAlreadyExists fake message");
            }

            [Fact]
            public void NotCall_Third_Validator_When_Second_Fails_And_A_BreakStep_Is_Setup()
            {
                // Arrange
                this.Arrange_Three_Validators_With_Second_Fails_FollowedBy_ABreakStep();

                // Act
                var reportResult = this.Processor.ProcessValidators(
                    this.ValidatorStepsMock,
                    this.CommandMock
                    );

                // Assert
                var valitatorStepList = this.ValidatorStepsMock.ToList();

                // validator 0 has been called
                valitatorStepList[0]
                    .GetValidator()
                    .Received(1)
                    .Validate(this.CommandMock);

                // validator 1 has been called and returns FALSE
                valitatorStepList[1]
                    .GetValidator()
                    .Received(1)
                    .Validate(this.CommandMock);

                // validator step 2 is a BREAK
                // so its validator is not called
                valitatorStepList[2]
                    .GetValidator()
                    .Received(0)
                    .Validate(Arg.Any<FakeCommand>());


                // as previous step is a BREAK
                // validator 3 is not called
                valitatorStepList[2]
                  .GetValidator()
                  .Received(0)
                  .Validate(Arg.Any<FakeCommand>());
            }

            [Fact]
            public void Returns_Expected_ValidatedCommands_Count_When_Second_Fails_And_A_BreakStep_Is_Setup()
            {
                // Arrange
                this.Arrange_Three_Validators_With_Second_Fails_FollowedBy_ABreakStep();

                // Act
                var reportResult = this.Processor.ProcessValidators(
                    this.ValidatorStepsMock,
                    this.CommandMock
                    );

                // Assert
                reportResult.ValidatedCommands.Should().HaveCount(2);
            }

            [Fact]
            public void Returns_Expected_ValidatedCommands_Properties_When_First_Fails_And_A_BreakStep_Is_Setup_AtThirdStep()
            {
                // Arrange
                this.Arrange_Three_Validators_With_First_Fail_And_Third_is_ABreakStep();

                // Act
                var reportResult = this.Processor.ProcessValidators(
                    this.ValidatorStepsMock,
                    this.CommandMock
                    );

                // Assert
                var validatedCommands = reportResult.ValidatedCommands.ToList();
                validatedCommands[0].IsValid.Should().BeFalse();
                validatedCommands[0].FailureMessage.Should().Be("messagebuilder UserAlreadyExists fake message");

                validatedCommands[1].IsValid.Should().BeTrue();
                validatedCommands[1].FailureMessage.Should().BeNull();
            }

            [Fact]
            public void Returns_Expected_ValidatedCommands_Properties_When_Second_Fails_And_A_BreakStep_Is_Setup()
            {
                // Arrange
                this.Arrange_Three_Validators_With_Second_Fails_FollowedBy_ABreakStep();

                // Act
                var reportResult = this.Processor.ProcessValidators(
                    this.ValidatorStepsMock,
                    this.CommandMock
                    );

                // Assert
                var validatedCommands = reportResult.ValidatedCommands.ToList();
                validatedCommands[0].IsValid.Should().BeTrue();
                validatedCommands[0].FailureMessage.Should().BeNull();

                validatedCommands[1].IsValid.Should().BeFalse();
                validatedCommands[1].FailureMessage.Should().Be("messagebuilder UserAlreadyExists fake message");
            }

            private void Arrange_Three_Validators_With_Second_Fails_FollowedBy_ABreakStep()
            {
                // Arrange  3 validationStep with validator that returns TRUE
                var validators = ValidationStepsBuilderHelper<FakeCommand>.CreateValidationStepsBuilderHelper()
                    .AddValidationStep(this.CommandMock, isValidatorValid: true)
                    .AddValidationStep(this.CommandMock, isValidatorValid: false, messageFailureCode: MessageCode.UserAlreadyExists)
                    .AddValidationStep(this.CommandMock, canContinueIfValid: false)
                    .AddValidationStep(this.CommandMock, isValidatorValid: true)
                    .Build();

                this.ValidatorStepsMock = validators;
            }

            private void Arrange_Three_Validators_With_First_Fail_And_Third_is_ABreakStep()
            {
                // Arrange  3 validationStep with validator that returns TRUE
                var validators = ValidationStepsBuilderHelper<FakeCommand>.CreateValidationStepsBuilderHelper()
                    .AddValidationStep(this.CommandMock, isValidatorValid: false, messageFailureCode: MessageCode.UserAlreadyExists)
                    .AddValidationStep(this.CommandMock, isValidatorValid: true)
                    .AddValidationStep(this.CommandMock, canContinueIfValid: false)
                    .AddValidationStep(this.CommandMock, isValidatorValid: true)
                    .Build();

                this.ValidatorStepsMock = validators;
            }

            private void Arrange_Three_Validators_That_Succeeds()
            {
                // Arrange  3 validationStep with validator that returns TRUE
                var validators = ValidationStepsBuilderHelper<FakeCommand>.CreateValidationStepsBuilderHelper()
                    .AddValidationStep(this.CommandMock, isValidatorValid: true)
                    .AddValidationStep(this.CommandMock, isValidatorValid: true)
                    .AddValidationStep(this.CommandMock, isValidatorValid: true)
                    .Build();

                this.ValidatorStepsMock = validators;
            }

            private void Arrange_Three_Validators_With_One_Fails()
            {
                // Arrange  3 validationStep with one validator fails
                var validators = ValidationStepsBuilderHelper<FakeCommand>.CreateValidationStepsBuilderHelper()
                    .AddValidationStep(this.CommandMock, isValidatorValid: true)
                    .AddValidationStep(this.CommandMock, isValidatorValid: true)
                    .AddValidationStep(this.CommandMock, isValidatorValid: false, messageFailureCode: MessageCode.UserAlreadyExists)
                    .Build();

                this.ValidatorStepsMock = validators;
            }
        }
    }
}