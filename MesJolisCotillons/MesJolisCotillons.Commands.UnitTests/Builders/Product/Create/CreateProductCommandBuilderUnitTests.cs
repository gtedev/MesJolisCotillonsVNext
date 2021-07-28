using FluentAssertions;
using MesJolisCotillons.Commands.Builders.Product.Create;
using MesJolisCotillons.Commands.Product.Create;
using MesJolisCotillons.Contracts.Requests.Product.Create;
using Xunit;

namespace MesJolisCotillons.Commands.UnitTests.Builders.Product.Create
{
    public class CreateProductCommandBuilderUnitTests
    {
        public abstract class CreateProductCommandBuilderUnitTest
            : CommandBuilderTestBase<CreateProductRequest, CreateProductCommand, CreateProductCommandBuilder>
        {
            public CreateProductCommandBuilderUnitTest() : base()
            {
                this.CommandBuilder = new CreateProductCommandBuilder();
            }
        }

        public class BuildShould : CreateProductCommandBuilderUnitTest
        {
            [Fact]
            public void Returns_Command_NonNull()
            {
                // Arrange
                this.Request.Name = "MyName";
                this.Request.Description = "MyDescription";
                this.Request.Price = 20;
                this.Request.DisplayName = "MyDisplayName";

                // Act
                this.Command = this.CommandBuilder.Build(this.Request);

                // Assert
                this.Command.Should().NotBeNull();
            }

            [Fact]
            public void Returns_Command_withPropertiesSet()
            {
                // Arrange
                this.Request.Name = "MyName";
                this.Request.Description = "MyDescription";
                this.Request.Price = 20;
                this.Request.DisplayName = "MyDisplayName";

                // Act
                this.Command = this.CommandBuilder.Build(this.Request);

                // Assert
                this.Command.Name.Should().Be("MyName");
                this.Command.Description.Should().Be("MyDescription");
                this.Command.Price.Should().Be(20);
                this.Command.DisplayName.Should().Be("MyDisplayName");
            }
        }
    }
}
