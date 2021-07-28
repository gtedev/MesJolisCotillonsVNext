using FluentAssertions;
using MesJolisCotillons.Adapters.Product.Delete;
using MesJolisCotillons.Contracts.Requests.Product.Delete;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.DataAccess.Repositories.Repositories;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Adapters.UnitTests.Adapters.Product.Delete
{
    public class DeleteProductAdapterTests
    {
        public abstract class DeleteProductAdapterTest : AdapterTestBase<DeleteProductAdapter, DeleteProductRequest>
        {
            protected IProductRepository ProductRepositoryMock;

            public DeleteProductAdapterTest()
            {
                this.ProductRepositoryMock = Substitute.For<IProductRepository>();

                this.Adapter = new DeleteProductAdapter(this.ProductRepositoryMock);
            }
        }

        public class ExistingProductShould : DeleteProductAdapterTest
        {
            [Fact]
            public async Task BeSet_WhenProductExistsAsync()
            {
                // Arrange
                this.ProductRepositoryMock
                    .FindProduct(Arg.Any<int>())
                    .Returns(new ProductViewModel());

                // Act
                await this.Adapter.InitAdapter(this.Request);

                // Assert
                this.Adapter
                    .ExistingProduct
                    .Should().NotBeNull();
            }

            [Fact]
            public async Task BeSet_WhenProductDoesNotExistsAsync()
            {
                // Arrange
                this.ProductRepositoryMock
                    .FindProduct(Arg.Any<int>())
                    .Returns((ProductViewModel)null);

                // Act
                await this.Adapter.InitAdapter(this.Request);

                // Assert
                this.Adapter
                    .ExistingProduct
                    .Should().BeNull();
            }

            [Fact]
            public async Task Call_FindProduct_In_ProductRepositoryAsync()
            {
                // Arrange
                this.ProductRepositoryMock
                    .FindProduct(Arg.Any<int>())
                    .Returns((ProductViewModel)null);

                // Act
                await this.Adapter.InitAdapter(this.Request);

                // Assert
                await this.ProductRepositoryMock
                   .Received(1)
                   .FindProduct(Arg.Any<int>());                       
            }
        }
    }
}
