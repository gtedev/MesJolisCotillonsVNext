using FluentAssertions;
using MesJolisCotillons.Commands.Product.Create;
using MesJolisCotillons.Commands.Product.Get;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.DataAccess.Repositories.Repositories;
using MesJolisCotillons.Executors.Product.Create;
using MesJolisCotillons.Executors.Product.Get;
using MesJolisCotillons.Executors.Services;
using NSubstitute;
using NSubstitute.Core.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Executors.UnitTests.Product.Get
{
    public class ProductPagingServiceTests
    {
        public abstract class ProductPagingServiceTest
        {
            public ProductPagingServiceTest()
            {
                this.Service = new ProductPagingService();
            }

            protected ProductPagingService Service { get; set; }
        }

        public class GetPagedProductsShould : ProductPagingServiceTest
        {
            [Fact]
            public async Task Returns_First_Page_Expected_Count_When_More_Than_PageSize()
            {
                // Arrange
                var products = this.ArrangeNumberOfProducts(numberOfProduct: 50);
                var page = 1;
                var pageSize = 20;
                // Act
                var results = this.Service.GetPagedProducts(products, page, pageSize);

                // Assert
                results.Should().HaveCount(20);
            }

            [Fact]
            public async Task Returns_First_Page_Expected_Count_When_Less_Than_PageSize()
            {
                // Arrange
                var products = this.ArrangeNumberOfProducts(numberOfProduct: 10);
                var page = 1;
                var pageSize = 20;
                // Act
                var results = this.Service.GetPagedProducts(products, page, pageSize);

                // Assert
                results.Should().HaveCount(10);
            }

            [Fact]
            public async Task Returns_Second_Page_Expected_WithExpected_ProductIds()
            {
                // Arrange
                var products = this.ArrangeNumberOfProducts(numberOfProduct: 50);
                var page = 2;
                var pageSize = 20;
                // Act
                var results = this.Service.GetPagedProducts(products, page, pageSize);

                // Assert
                var expectedProductIds = this.GetExpectedProductsIds(21, 20);
                results
                    .Select(item => item.ProductId)
                    .Should().BeEquivalentTo(expectedProductIds);
            }

            [Fact]
            public async Task ReturnsThird_Page_Expected_WithExpected_ProductIds()
            {
                // Arrange
                var products = this.ArrangeNumberOfProducts(numberOfProduct: 50);
                var page = 3;
                var pageSize = 20;
                // Act
                var results = this.Service.GetPagedProducts(products, page, pageSize);

                // Assert
                var expectedProductIds = this.GetExpectedProductsIds(41, 10);
                results
                    .Select(item => item.ProductId)
                    .Should().BeEquivalentTo(expectedProductIds);
            }

            [Fact]
            public async Task ReturnsAll_Products_WithExpected_ProductIds() 
            {
                // Arrange
                var products = this.ArrangeNumberOfProducts(numberOfProduct: 50);
                var page = 1;
                var pageSize = 50;

                // Act
                var results = this.Service.GetPagedProducts(products, page, pageSize);

                // Assert
                var expectedProductIds = this.GetExpectedProductsIds(1, 50);
                results
                    .Select(item => item.ProductId)
                    .Should().BeEquivalentTo(expectedProductIds);
            }


            private IReadOnlyCollection<ProductViewModel> ArrangeNumberOfProducts(int numberOfProduct = 20)
            {
                var products = new List<ProductViewModel>();

                for (int i = 1; i <= numberOfProduct; i++)
                {
                    var productName = $"Product_{i}";
                    var productView = new ProductViewModel(
                        i,
                        productName,
                        $"MyDescription for {productName}",
                        10,
                        productName,
                        new List<string> { "123", "456" });

                    products.Add(productView);
                }

                return products;
            }

            private IEnumerable<int> GetExpectedProductsIds(int startProductId, int numberOfProduct = 20)
            {
                var products = new List<int>();
                var currentId = startProductId;

                for (int i = 1; i <= numberOfProduct; i++)
                {
                    products.Add(currentId);
                    currentId = currentId + 1;
                }

                return products;
            }
        }
    }
}
