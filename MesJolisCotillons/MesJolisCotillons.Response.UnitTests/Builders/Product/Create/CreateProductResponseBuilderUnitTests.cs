using FluentAssertions;
using MesJolisCotillons.Commands.Product.Create;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Contracts.Responses.Product.Create;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.Response.Builders.Product.Create;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Response.UnitTests.Builders.Product.Create
{
    public class CreateProductResponseBuilderUnitTests
    {
        public abstract class CreateProductResponseBuilderUnitTest
            : ResponseBuilderTestBase<CreateProductCommand, CreateProductResponse, CreateProductResponseBuilder>
        {
            public CreateProductResponseBuilderUnitTest() : base()
            {
                this.Command = new CreateProductCommand();
                this.ResponseBuilder = new CreateProductResponseBuilder(this.MessagesServiceMock);
                this.ValidationReportMock.Command.Returns(this.Command);
            }
        }

        public class BuildShould : CreateProductResponseBuilderUnitTest
        {
            [Fact]
            public void Call_GetMessage_From_MessageService_WithExpectedProperties()
            {
                // Arrange
                this.Command.Name = "MyProductName";
                var product = new ProductViewModel();
                this.Command.ProductViewResolver = () => product;

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                this.MessagesServiceMock
                    .Received()
                    .GetMessage(
                        Arg.Is<MessageCode>(messageCode => messageCode == MessageCode.CreateProductSuccess),
                        Arg.Is<string>(commandName => commandName == "MyProductName"));
            }

            [Fact]
            public void Set_ProductView_AfterCallingProductViewResolver()
            {
                // Arrange
                this.Command.Name = "MyProductName";
                var product = new ProductViewModel();
                this.Command.ProductViewResolver = () => product;

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                response.Product.Should().BeSameAs(product);
            }

            [Fact]
            public void Returns_Expected_Message()
            {
                // Arrange
                this.ValidationReportMock.OperationName.Returns("MyOperationName");
                var product = new ProductViewModel();
                this.Command.ProductViewResolver = () => product;

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
