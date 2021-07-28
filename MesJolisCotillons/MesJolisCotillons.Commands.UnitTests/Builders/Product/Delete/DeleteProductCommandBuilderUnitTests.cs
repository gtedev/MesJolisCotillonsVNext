using FluentAssertions;
using MesJolisCotillons.Adapters.Product.Delete;
using MesJolisCotillons.Commands.Builders.Product.Delete;
using MesJolisCotillons.Commands.Product.Delete;
using MesJolisCotillons.Contracts.Requests.Product.Delete;
using MesJolisCotillons.Contracts.ViewModels.Product;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Commands.UnitTests.Builders.Product.Delete
{
    public class DeleteProductCommandBuilderUnitTests
    {
        public abstract class DeleteProductCommandBuilderUnitTest
            : CommandBuilderTestBase<IDeleteProductAdapter, DeleteProductRequest, DeleteProductCommand, DeleteProductCommandBuilder>
        {
            public DeleteProductCommandBuilderUnitTest() : base()
            {
                this.CommandBuilder = new DeleteProductCommandBuilder();
            }
        }

        public class BuildShould : DeleteProductCommandBuilderUnitTest
        {
            [Fact]
            public void Returns_Command_NonNull()
            {
                // Arrange
                var product = new ProductViewModel();
                this.Request.ProductId = 1;
                this.Adapter.ExistingProduct.Returns(product);

                // Act
                this.Command = this.CommandBuilder.Build(this.Adapter, this.Request);

                // Assert
                this.Command.ExistingProduct.Should().NotBeNull();
            }

            [Fact]
            public void Returns_Command_withPropertiesSet()
            {
                // Arrange
                var product = new ProductViewModel();
                this.Request.ProductId = 1;
                this.Adapter.ExistingProduct.Returns(product);

                // Act
                this.Command = this.CommandBuilder.Build(this.Adapter, this.Request);

                // Assert
                this.Command.ProductId.Should().Be(1);
                this.Command.ExistingProduct.Should().Be(product);
            }
        }
    }
}
