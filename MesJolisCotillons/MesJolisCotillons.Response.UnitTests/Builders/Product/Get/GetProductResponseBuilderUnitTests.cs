using FluentAssertions;
using MesJolisCotillons.Commands.Product.Delete;
using MesJolisCotillons.Commands.Product.Get;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Contracts.Responses.Product.Get;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.Response.Builders.Product.Get;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Response.UnitTests.Builders.Product.Get
{
    public class GetProductResponseBuilderUnitTests
    {
        public abstract class GetProductResponseBuilderUnitTest
            : ResponseBuilderTestBase<GetProductCommand, GetProductResponse, GetProductResponseBuilder>
        {
            public GetProductResponseBuilderUnitTest() : base()
            {
                this.ResponseBuilder = new GetProductResponseBuilder(this.MessagesServiceMock);
            }
        }

        public class BuildShould : GetProductResponseBuilderUnitTest
        {
            [Fact]
            public void Call_GetMessage_From_MessageService_WithExpectedProperties()
            {
                // Arrange
                var existinProduct = new ProductViewModel();
                this.Command = new GetProductCommand(1, existinProduct);
                this.ValidationReportMock.Command.Returns(this.Command);

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                this.MessagesServiceMock
                    .Received(0);
            }

            [Fact]
            public void Returns_Expected_Message()
            {
                // Arrange
                var existinProduct = new ProductViewModel();
                this.Command = new GetProductCommand(1, existinProduct);
                this.ValidationReportMock.Command.Returns(this.Command);

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                response.Should().NotBeNull();
                response.Messages.Should().HaveCount(0);
                response.Success.Should().BeTrue();
            }

            [Fact]
            public void Returns_Expected_ProductViewModel()
            {
                // Arrange
                var existinProduct = new ProductViewModel();
                this.Command = new GetProductCommand(1, existinProduct);
                this.ValidationReportMock.Command.Returns(this.Command);

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                response.Product.Should().BeSameAs(existinProduct);
            }
        }
    }
}
