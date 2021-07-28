using FluentAssertions;
using MesJolisCotillons.Adapters.Product.Get;
using MesJolisCotillons.Commands.Builders.Product.Delete;
using MesJolisCotillons.Commands.Builders.Product.Get;
using MesJolisCotillons.Commands.Product.Get;
using MesJolisCotillons.Contracts.Requests.Product.Get;
using MesJolisCotillons.Contracts.ViewModels.Product;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Commands.UnitTests.Builders.Product.Delete
{
    public class GetProductCommandBuilderUnitTests
    {
        public abstract class GetProductCommandBuilderUnitTest
            : CommandBuilderTestBase<IGetProductAdapter, GetProductRequest, GetProductCommand, GetProductCommandBuilder>
        {
            public GetProductCommandBuilderUnitTest() : base()
            {
                this.CommandBuilder = new GetProductCommandBuilder();
            }
        }

        public class BuildShould : GetProductCommandBuilderUnitTest
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
