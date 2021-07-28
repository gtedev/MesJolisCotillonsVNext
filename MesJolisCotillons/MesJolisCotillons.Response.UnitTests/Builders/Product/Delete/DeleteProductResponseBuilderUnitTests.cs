using FluentAssertions;
using MesJolisCotillons.Commands.Product.Create;
using MesJolisCotillons.Commands.Product.Delete;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Contracts.Responses.Product.Create;
using MesJolisCotillons.Contracts.Responses.Product.Delete;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.Response.Builders.Product.Create;
using MesJolisCotillons.Response.Builders.Product.Delete;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Response.UnitTests.Builders.Product.Delete
{
    public class DeleteProductResponseBuilderUnitTests
    {
        public abstract class DeleteProductResponseBuilderUnitTest
            : ResponseBuilderTestBase<DeleteProductCommand, DeleteProductResponse, DeleteProductResponseBuilder>
        {
            public DeleteProductResponseBuilderUnitTest() : base()
            {
                this.ResponseBuilder = new DeleteProductResponseBuilder(this.MessagesServiceMock);
            }
        }

        public class BuildShould : DeleteProductResponseBuilderUnitTest
        {
            [Fact]
            public void Call_GetMessage_From_MessageService_WithExpectedProperties()
            {
                // Arrange
                var existinProduct = new ProductViewModel();
                this.Command = new DeleteProductCommand(1, existinProduct);
                this.ValidationReportMock.Command.Returns(this.Command);

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                this.MessagesServiceMock
                    .Received()
                    .GetMessage(
                        Arg.Is<MessageCode>(messageCode => messageCode == MessageCode.DeleteProductSuccess),
                        Arg.Is<string>(productId => productId == "1"));
            }

            [Fact]
            public void Returns_Expected_Message()
            {
                // Arrange
                var existinProduct = new ProductViewModel();
                this.Command = new DeleteProductCommand(1, existinProduct);
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
