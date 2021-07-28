using FluentAssertions;
using MesJolisCotillons.Commands.Product.Get;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Contracts.Responses.Product.Get;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.Response.Builders.Product.Get;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace MesJolisCotillons.Response.UnitTests.Builders.Product.Get
{
    public class GetProductsResponseBuilderUnitTests
    {
        public abstract class GetProductsResponseBuilderUnitTest
            : ResponseBuilderTestBase<GetProductsCommand, GetProductsResponse, GetProductsResponseBuilder>
        {
            public GetProductsResponseBuilderUnitTest() : base()
            {
                this.ResponseBuilder = new GetProductsResponseBuilder(this.MessagesServiceMock);

                this.Command = new GetProductsCommand(
                    1,
                    20,
                    new List<int> { 1, 2, 3 },
                    new List<int> { 1000 },
                    true);

                this.ValidationReportMock.Command.Returns(this.Command);
            }
        }

        public class BuildShould : GetProductsResponseBuilderUnitTest
        {
            [Fact]
            public void Returns_Expected_ProductsCount()
            {
                // Arrange
                var productViewOne = new ProductViewModel(
                    1,
                    "ProductOne",
                    "MyDescription for ProductOne",
                    10,
                    "ProductOne",
                    new List<string> { "123", "456" });

                var productViewTwo = new ProductViewModel(
                    2,
                    "ProductTwo",
                    "MyDescription for ProductTwo",
                    10,
                    "ProductTwo",
                    new List<string> { "123", "456" });

                this.ValidationReportMock.OperationName.Returns("MyOperationName");
                var products = new List<ProductViewModel>
                {
                    productViewOne,
                    productViewTwo
                };

                this.Command.ProductsResult = products;

                // Act
                var response = this.ResponseBuilder.Build(this.Response, this.ValidationReportMock);

                // Assert
                response.Should().NotBeNull();
                response.Messages.Should().HaveCount(0);
                response.Products.Should().HaveCount(2);
                response.Success.Should().BeTrue();
            }
        }
    }
}
