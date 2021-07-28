using FluentAssertions;
using MesJolisCotillons.Commands.Product.Create;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.DataAccess.Repositories.Repositories;
using MesJolisCotillons.Executors.Product.Create;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Executors.UnitTests.Product.Create
{
    public class CreateProductExecutorUnitTests
    {
        public abstract class CreateProductExecutorUnitTest
            : ExecutorTestBase<CreateProductCommand, IProductRepository, CreateProductExecutor>
        {
            public CreateProductExecutorUnitTest() : base()
            {
                this.Executor = new CreateProductExecutor(this.RepositoryMock);
            }
        }

        public class ExecuteShould : CreateProductExecutorUnitTest
        {
            [Fact]
            public async Task Call_CreateProduct_From_Repository_WithExpectedPropertiesAsync()
            {
                // Arrange
                this.Command = new CreateProductCommand
                {
                    Name = "MyProduct",
                    Description = "MyDescription",
                    Price = 20,
                    DisplayName = "MyDisplayProduct"
                };

                // Act
                await this.Executor.ExecuteAsync(this.Command);

                // Assert
                await this.RepositoryMock
                    .Received()
                    .CreateProduct(
                        Arg.Is<string>(name => name == "MyProduct"),
                        Arg.Is<string>(description => description == "MyDescription"),
                        Arg.Is<decimal?>(price => price == 20),
                        Arg.Is<string>(displayName => displayName == "MyDisplayProduct"));
            }

            [Fact]
            public async Task Set_ProductIdResolverAsync()
            {
                // Arrange
                this.Command = new CreateProductCommand
                {
                    Name = "MyProduct",
                    Description = "MyDescription",
                    Price = 20,
                    DisplayName = "MyDisplayProduct"
                };

                Func<ProductViewModel> productResolver = () => { return new ProductViewModel(); };

                this.RepositoryMock
                    .CreateProduct(
                     Arg.Any<string>(),
                     Arg.Any<string>(),
                     Arg.Any<decimal?>(),
                     Arg.Any<string>())
                    .Returns(productResolver);

                //Pre-Assert
                this.Command.ProductViewResolver.Should().BeNull();

                // Act
                await this.Executor.ExecuteAsync(this.Command);

                // Assert
                this.Command.ProductViewResolver.Should().NotBeNull();
                var productResult = this.Command.ProductViewResolver();

                this.Command.ProductViewResolver
                    .Should()
                    .BeSameAs(productResolver);
            }
        }
    }
}
