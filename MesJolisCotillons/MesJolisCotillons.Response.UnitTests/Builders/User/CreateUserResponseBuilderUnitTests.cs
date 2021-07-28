using FluentAssertions;
using MesJolisCotillons.Commands.Product.Get;
using MesJolisCotillons.Commands.User.Create;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Contracts.Responses.Product.Get;
using MesJolisCotillons.Contracts.Responses.User.Create;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.Response.Builders.Product.Get;
using MesJolisCotillons.Response.Builders.User.Create;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Response.UnitTests.Builders.User.Create
{
    public class CreateUserResponseBuilderUnitTests
    {
        public abstract class CreateUserResponseBuilderUnitTest
            : ResponseBuilderTestBase<CreateUserCommand, CreateUserResponse, CreateUserResponseBuilder>
        {
            public CreateUserResponseBuilderUnitTest() : base()
            {
                this.ResponseBuilder = new CreateUserResponseBuilder(this.MessagesServiceMock);
            }
        }

        public class BuildShould : CreateUserResponseBuilderUnitTest
        {
            [Fact]
            public void Call_GetMessage_From_MessageService_WithExpectedProperties()
            {
                // Arrange
                this.Command = new CreateUserCommand { Email = "bruce.lee@gmail.com" };
                this.ValidationReportMock.Command.Returns(this.Command);

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                this.MessagesServiceMock
                    .Received(1)
                    .GetMessage(
                        Arg.Is<MessageCode>(messageCode => messageCode == MessageCode.DefaultReponseSuccess),
                        Arg.Is<string>(email => email == "bruce.lee@gmail.com"));
            }

            [Fact]
            public void Returns_Expected_Message()
            {
                // Arrange
                this.Command = new CreateUserCommand { Email = "bruce.lee@gmail.com" };
                this.ValidationReportMock.Command.Returns(this.Command);

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                response.Should().NotBeNull();
                response.Messages.Should().HaveCount(1);
                response.Success.Should().BeTrue();
            }
        }
    }
}
