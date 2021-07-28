using FluentAssertions;
using MesJolisCotillons.Adapters.Product.Get;
using MesJolisCotillons.Commands.Builders.Product.Get;
using MesJolisCotillons.Commands.Product.Get;
using MesJolisCotillons.Contracts.Requests.Product.Get;
using MesJolisCotillons.Contracts.ViewModels.Product;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MesJolisCotillons.Commands.UnitTests.Builders.Product.Get
{
    public class GetProductsCommandBuilderUnitTests
    {
        public abstract class GetProductsCommandBuilderUnitTest
            : CommandBuilderTestBase<IGetProductsAdapter, GetProductsRequest, GetProductsCommand, GetProductsCommandBuilder>
        {
            public GetProductsCommandBuilderUnitTest() : base()
            {
                this.CommandBuilder = new GetProductsCommandBuilder();
            }
        }

        public class BuildShould : GetProductsCommandBuilderUnitTest
        {
            [Fact]
            public void Returns_Command_NonNull()
            {
                // Arrange
                IReadOnlyCollection<CategoryViewModel> categories = new List<CategoryViewModel>
                {
                    new CategoryViewModel(1,"category1","category1")
                };

                this.Adapter.ExistingCategories.Returns(categories);

                this.Request = new GetProductsRequest
                {
                    Page = 1,
                    PageSize = 20,
                    IncludeFirstPicture = true,
                    ProductCategories = new List<int> { 1 }
                };

                // Act
                this.Command = this.CommandBuilder.Build(this.Adapter, this.Request);

                // Assert
                this.Command.Should().NotBeNull();
            }

            [Fact]
            public void Returns_Command_WithExpectedNonExistingCategories_Count_WhenOneDoesNotExist()
            {
                // Arrange
                IReadOnlyCollection<CategoryViewModel> categories = new List<CategoryViewModel>
                {
                    new CategoryViewModel(1,"category1","category1"),
                    new CategoryViewModel(2,"category2","category2")
                };

                this.Adapter.ExistingCategories.Returns(categories);

                this.Request = new GetProductsRequest
                {
                    Page = 1,
                    PageSize = 20,
                    IncludeFirstPicture = true,
                    ProductCategories = new List<int> { 1, 2, 3 }
                };

                // Act
                this.Command = this.CommandBuilder.Build(this.Adapter, this.Request);

                // Assert
                this.Command
                    .NonExistingCategories
                    .Should()
                    .HaveCount(1);
            }

            [Fact]
            public void Returns_Command_WithExpectedNonExistingCategories_WhenOneDoesNotExist()
            {
                // Arrange
                IReadOnlyCollection<CategoryViewModel> categories = new List<CategoryViewModel>
                {
                    new CategoryViewModel(1,"category1","category1"),
                    new CategoryViewModel(2,"category2","category2")
                };

                this.Adapter.ExistingCategories.Returns(categories);

                this.Request = new GetProductsRequest
                {
                    Page = 1,
                    PageSize = 20,
                    IncludeFirstPicture = true,
                    ProductCategories = new List<int> { 1, 2, 3 }
                };

                // Act
                this.Command = this.CommandBuilder.Build(this.Adapter, this.Request);

                // Assert
                this.Command
                    .NonExistingCategories
                    .FirstOrDefault()
                    .Should()
                    .Be(3);
            }

            [Fact]
            public void Returns_Command_WithExpectedNonExistingCategories_Count_WhenAllExist()
            {
                // Arrange
                IReadOnlyCollection<CategoryViewModel> categories = new List<CategoryViewModel>
                {
                    new CategoryViewModel(1,"category1","category1"),
                    new CategoryViewModel(2,"category2","category2")
                };

                this.Adapter.ExistingCategories.Returns(categories);

                this.Request = new GetProductsRequest
                {
                    Page = 1,
                    PageSize = 20,
                    IncludeFirstPicture = true,
                    ProductCategories = new List<int> { 1, 2 }
                };

                // Act
                this.Command = this.CommandBuilder.Build(this.Adapter, this.Request);

                // Assert
                this.Command
                    .NonExistingCategories
                    .Should()
                    .HaveCount(0);
            }

            [Fact]
            public void Returns_Command_WithProductsResult_NotSet()
            {
                // Arrange
                IReadOnlyCollection<CategoryViewModel> categories = new List<CategoryViewModel>
                {
                    new CategoryViewModel(1,"category1","category1"),
                    new CategoryViewModel(2,"category2","category2")
                };

                this.Adapter.ExistingCategories.Returns(categories);

                this.Request = new GetProductsRequest
                {
                    Page = 1,
                    PageSize = 20,
                    IncludeFirstPicture = true,
                    ProductCategories = new List<int> { 1, 2 }
                };

                // Act
                this.Command = this.CommandBuilder.Build(this.Adapter, this.Request);

                // Assert
                this.Command
                    .ProductsResult
                    .Should()
                    .BeNull();
            }

            [Fact]
            public void Returns_Command_WithBasicProperties_Set()
            {
                // Arrange
                IReadOnlyCollection<CategoryViewModel> categories = new List<CategoryViewModel>
                {
                    new CategoryViewModel(1,"category1","category1"),
                    new CategoryViewModel(2,"category2","category2")
                };

                this.Adapter.ExistingCategories.Returns(categories);

                this.Request = new GetProductsRequest
                {
                    Page = 1,
                    PageSize = 20,
                    IncludeFirstPicture = true,
                    ProductCategories = new List<int> { 1, 2 }
                };

                // Act
                this.Command = this.CommandBuilder.Build(this.Adapter, this.Request);

                // Assert
                this.Command.Page.Should().Be(this.Request.Page);
                this.Command.PageSize.Should().Be(this.Request.PageSize);
                this.Command.IncludeFirstPicture.Should().BeTrue();
                this.Command.ProductCategories.Should().BeEquivalentTo(this.Request.ProductCategories);
            }
        }
    }
}
