using FluentAssertions;
using MesJolisCotillons.Adapters.Product.Get;
using MesJolisCotillons.Contracts.Requests.Product.Get;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.DataAccess.Repositories.Repositories;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Adapters.UnitTests.Adapters.Product.Get
{
    public class GetProductAdapterTests
    {
        public abstract class GetProductAdapterTest : AdapterTestBase<GetProductAdapter, GetProductRequest>
        {
            protected IProductRepository ProductRepositoryMock;

            public GetProductAdapterTest()
            {
                this.ProductRepositoryMock = Substitute.For<IProductRepository>();

                this.Adapter = new GetProductAdapter(this.ProductRepositoryMock);
            }
        }

        public class ExistingProductShould : GetProductAdapterTest
        {
            [Fact]
            public async Task BeSet_WhenProductExistsAsync()
            {
                // Arrange
                this.ProductRepositoryMock
                    .FindProduct(Arg.Any<int>(), Arg.Any<bool>())
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
                    .FindProduct(Arg.Any<int>(), Arg.Any<bool>())
                    .Returns((ProductViewModel)null);

                // Act
                await this.Adapter.InitAdapter(this.Request);

                // Assert
                await this.ProductRepositoryMock
                   .Received(1)
                   .FindProduct(Arg.Any<int>(), Arg.Any<bool>());
            }
        }
    }
}
