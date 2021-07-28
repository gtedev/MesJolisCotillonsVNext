using FluentAssertions;
using MesJolisCotillons.Common.UnitTests.Fake;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Response.Builders.Default;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Response.UnitTests.Builders.Default
{
    public class DefaultResponseBuilderUnitTests
    {
        public abstract class DefaultResponseBuilderUnitTest
            : ResponseBuilderTestBase<FakeCommand, FakeResponse, DefaultResponseBuilder<FakeCommand, FakeResponse>>
        {
            public DefaultResponseBuilderUnitTest() : base()
            {
                this.Command = new FakeCommand();
                this.ResponseBuilder = new DefaultResponseBuilder<FakeCommand, FakeResponse>(this.MessagesServiceMock);
            }
        }

        public class BuildShould : DefaultResponseBuilderUnitTest
        {
            [Fact]
            public void Call_GetMessage_From_MessageService_WithExpectedProperties()
            {
                // Arrange
                this.ValidationReportMock.OperationName.Returns("MyOperationName");

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                this.MessagesServiceMock
                    .Received()
                    .GetMessage(
                        Arg.Is<MessageCode>(messageCode => messageCode == MessageCode.DefaultReponseSuccess),
                        Arg.Is<string>(operationName => operationName == "MyOperationName"));
            }

            [Fact]
            public void Returns_Expected_Message()
            {
                // Arrange
                this.ValidationReportMock.OperationName.Returns("MyOperationName");

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
