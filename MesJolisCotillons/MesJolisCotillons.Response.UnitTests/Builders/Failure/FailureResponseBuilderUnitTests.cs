using FluentAssertions;
using MesJolisCotillons.Common.UnitTests.Fake;
using MesJolisCotillons.Response.Builders.Default;
using MesJolisCotillons.Validation.Validators;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace MesJolisCotillons.Response.UnitTests.Builders.Failure
{
    public class FailureResponseBuilderUnitTests
    {
        public abstract class FailureResponseBuilderUnitTest
            : ResponseBuilderTestBase<FakeCommand, FakeResponse, FailureResponseBuilder<FakeCommand, FakeResponse>>
        {
            public FailureResponseBuilderUnitTest() : base()
            {
                this.Command = new FakeCommand();
                this.ResponseBuilder = new FailureResponseBuilder<FakeCommand, FakeResponse>(this.MessagesServiceMock);
            }
        }

        public class BuildShould : FailureResponseBuilderUnitTest
        {
            [Fact]
            public void NotCall_GetMessage_From_MessageService()
            {
                // Arrange

                // Act
                this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                this.MessagesServiceMock
                    .Received(0);
            }

            [Fact]
            public void Returns_ExpectedCount_ErrorMessages()
            {
                // Arrange 4 validatedCommand, with 3 false
                var validatedCommandOne = new ValidatedCommand<FakeCommand>(true, new FakeCommand(), "FakeOperation");
                var validatedCommandTwo = new ValidatedCommand<FakeCommand>(false, new FakeCommand(), "FakeOperation");
                var validatedCommandThree = new ValidatedCommand<FakeCommand>(false, new FakeCommand(), "FakeOperation");
                var validatedCommandFour = new ValidatedCommand<FakeCommand>(false, new FakeCommand(), "FakeOperation");

                var validatedCommands = new List<ValidatedCommand<FakeCommand>>
                {
                    validatedCommandOne,
                    validatedCommandTwo,
                    validatedCommandThree,
                    validatedCommandFour
                };

                this.ValidationReportMock
                    .ValidatedCommands.Returns(validatedCommands);

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                response.Should().NotBeNull();
                response.Messages.Should().HaveCount(3);
            }

            [Fact]
            public void Returns_ExpectedErrorMessages_WithProperties()
            {
                // Arrange 4 validatedCommand, with 3 false
                var validatedCommandOne = new ValidatedCommand<FakeCommand>(true, new FakeCommand(), "FakeOperation", null);
                var validatedCommandTwo = new ValidatedCommand<FakeCommand>(false, new FakeCommand(), "FakeOperation", "FailureTwo");
                var validatedCommandThree = new ValidatedCommand<FakeCommand>(false, new FakeCommand(), "FakeOperation", "FailureThree");
                var validatedCommandFour = new ValidatedCommand<FakeCommand>(false, new FakeCommand(), "FakeOperation", "FailureFour");

                var validatedCommands = new List<ValidatedCommand<FakeCommand>>
                {
                    validatedCommandOne,
                    validatedCommandTwo,
                    validatedCommandThree,
                    validatedCommandFour
                };

                this.ValidationReportMock
                    .ValidatedCommands.Returns(validatedCommands);

                this.Response = new FakeResponse(false);

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                response.Messages[0].Should().Be("FailureTwo");
                response.Messages[1].Should().Be("FailureThree");
                response.Messages[2].Should().Be("FailureFour");
                response.Success.Should().Be(false);
            }
        }
    }
}
