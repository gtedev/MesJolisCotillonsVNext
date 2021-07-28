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
    public class GetProductsExecutorTests
    {
        public abstract class GetProductsExecutorTest
            : ExecutorTestBase<GetProductsCommand, IProductRepository, GetProductsExecutor>
        {
            public GetProductsExecutorTest() : base()
            {
                this.ProductPagingServiceMock = Substitute.For<IProductPagingService>();

                this.Executor = new GetProductsExecutor(
                    this.RepositoryMock,
                    this.ProductPagingServiceMock);
            }

            protected IProductPagingService ProductPagingServiceMock { get; set; }
        }

        public class ExecuteShould : GetProductsExecutorTest
        {
            [Fact]
            public async Task Call_FindAllProduct_From_Repository_WithExpectedPropertiesAsync()
            {
                // Arrange
                IReadOnlyCollection<int> categoriesFilter = new List<int> { 1 };
                this.Command = new GetProductsCommand(
                    1,
                    20,
                    categoriesFilter,
                    new List<int> { 1000 },
                    true);

                // Act
                await this.Executor.ExecuteAsync(this.Command);

                // Assert
                await this.RepositoryMock
                    .Received(1)
                    .FindAllProducts(
                        Arg.Is<bool>(includeFirstPicture => includeFirstPicture == true),
                        Arg.Is<IReadOnlyCollection<int>>(cf => cf.Count == 1 && cf.ElementAt(0) == 1));
            }

            [Fact]
            public async Task Call_GetPagedProduct_From_ProductPagingService_WithExpectedPropertiesAsync()
            {
                // Arrange
                IReadOnlyCollection<int> categoriesFilter = new List<int> { 1 };
                this.Command = new GetProductsCommand(
                    1,
                    20,
                    categoriesFilter,
                    new List<int> { 1000 },
                    true);

                IReadOnlyCollection<ProductViewModel> list = new List<ProductViewModel> { new ProductViewModel() };
                this.RepositoryMock
                    .FindAllProducts(Arg.Any<bool>(), Arg.Any<IReadOnlyCollection<int>>())
                    .Returns(Task.FromResult(list));

                // Act
                await this.Executor.ExecuteAsync(this.Command);

                // Assert
                this.ProductPagingServiceMock
                    .Received(1)
                    .GetPagedProducts(
                        Arg.Is<IReadOnlyCollection<ProductViewModel>>(products => products.Count == 1),
                        Arg.Is<int>(page => page == 1),
                        Arg.Is<int>(pageSize => pageSize == 20));
            }

            [Fact]
            public async Task BeSet_ProductResult_In_Command()
            {
                // Arrange
                IReadOnlyCollection<int> categoriesFilter = new List<int> { 1 };
                this.Command = new GetProductsCommand(
                    1,
                    20,
                    categoriesFilter,
                    new List<int> { 1000 },
                    true);

                // Act
                await this.Executor.ExecuteAsync(this.Command);

                // Assert
                this.Command.ProductsResult
                    .Should()
                    .NotBeNull();
            }
        }
    }
}
