using FluentAssertions;
using MesJolisCotillons.Adapters.Product.Get;
using MesJolisCotillons.Contracts.Requests.Product.Get;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.DataAccess.Repositories.Repositories;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Adapters.UnitTests.Adapters.Product.Get
{
    public class GetProductsAdapterTests
    {
        public abstract class GetProductsAdapterTest : AdapterTestBase<GetProductsAdapter, GetProductsRequest>
        {
            protected IProductRepository ProductRepositoryMock;

            public GetProductsAdapterTest()
            {
                this.ProductRepositoryMock = Substitute.For<IProductRepository>();

                this.Adapter = new GetProductsAdapter(this.ProductRepositoryMock);
            }
        }

        public class ExistingUserShould : GetProductsAdapterTest
        {
            [Fact]
            public async Task BeSet_WhenUserExistsAsync()
            {
                // Arrange
                IReadOnlyCollection<CategoryViewModel> categories = new List<CategoryViewModel>
                {
                    new CategoryViewModel(1,"category1","category1"),
                    new CategoryViewModel(2,"category2","category2")
                };

                this.ProductRepositoryMock
                    .FindAllCategories()
                    .Returns(Task.FromResult(categories));

                // Act
                await this.Adapter.InitAdapter(this.Request);

                // Assert
                this.Adapter
                    .ExistingCategories
                    .Should().NotBeNull();
            }
        }
    }
}
