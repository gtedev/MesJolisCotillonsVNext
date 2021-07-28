using FluentAssertions;
using MesJolisCotillons.Commands.TemplateNamespace;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Contracts.Responses.TemplateNamespace;
using MesJolisCotillons.Response.Builders.TemplateNamespace;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Response.UnitTests.Builders.TemplateNamespace
{
    public class TemplateOperationNameResponseBuilderUnitTests
    {
        public abstract class TemplateOperationNameResponseBuilderUnitTest
            : ResponseBuilderTestBase<TemplateOperationNameCommand, TemplateOperationNameResponse, TemplateOperationNameResponseBuilder>
        {
            public TemplateOperationNameResponseBuilderUnitTest() : base()
            {
                this.Command = new TemplateOperationNameCommand();
                this.ResponseBuilder = new TemplateOperationNameResponseBuilder(this.MessagesServiceMock);
                this.ValidationReportMock.Command.Returns(this.Command);
            }
        }

        public class BuildShould : TemplateOperationNameResponseBuilderUnitTest
        {
            [Fact]
            public void Call_GetMessage_From_MessageService_WithExpectedProperties()
            {
                // Arrange
                //this.Command.Name = "MyProductName";
                //var product = new ProductViewModel();
                //this.Command.ProductViewResolver = () => product;

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                //this.MessagesServiceMock
                //    .Received()
                //    .GetMessage(
                //        Arg.Is<MessageCode>(messageCode => messageCode == MessageCode.TemplateOperationNameSuccess),
                //        Arg.Is<string>(commandName => commandName == "MyProductName"));
            }

            [Fact]
            public void Returns_Expected_Message()
            {
                // Arrange
                //this.ValidationReportMock.OperationName.Returns("MyOperationName");
                //var product = new ProductViewModel();
                //this.Command.ProductViewResolver = () => product;

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
