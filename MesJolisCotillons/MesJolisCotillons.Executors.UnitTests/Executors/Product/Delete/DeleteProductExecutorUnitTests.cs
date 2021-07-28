using MesJolisCotillons.Commands.Product.Delete;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.DataAccess.Repositories.Repositories;
using MesJolisCotillons.Executors.Product.Delete;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Executors.UnitTests.Product.Delete
{
    public class DeleteProductExecutorUnitTests
    {
        public abstract class DeleteProductExecutorUnitTest
            : ExecutorTestBase<DeleteProductCommand, IProductRepository, DeleteProductExecutor>
        {
            public DeleteProductExecutorUnitTest() : base()
            {
                this.Executor = new DeleteProductExecutor(this.RepositoryMock);
            }
        }

        public class ExecuteShould : DeleteProductExecutorUnitTest
        {
            [Fact]
            public async Task Call_DeleteProduct_From_Repository_WithExpectedPropertiesAsync()
            {
                // Arrange
                this.Command = new DeleteProductCommand(1, new ProductViewModel());

                // Act
                await this.Executor.ExecuteAsync(this.Command);

                // Assert
                await this.RepositoryMock
                    .Received()
                    .DeleteProduct(
                        Arg.Is<int>(id => id == 1));
            }
        }
    }
}
