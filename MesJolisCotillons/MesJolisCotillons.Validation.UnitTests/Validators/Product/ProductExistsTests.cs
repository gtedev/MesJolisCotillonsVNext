using FluentAssertions;
using MesJolisCotillons.Commands.Product;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.Validation.Validators.Product;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Validation.UnitTests.Validators.Product
{
    public class ProductExistsTests : ValidatorTestBase<ProductExists, IExistingProductCommand>
    {
        public abstract class ProductExistsTest : ValidatorTestBase<ProductExists, IExistingProductCommand>
        {
        }

        public class MessageCodeShould : ProductExistsTest
        {
            [Fact]
            public void BeCorrect() => this.AssertMessageFailureCode(MessageCode.ProductDoesNotExist);
        }

        public class ValidateShould : ProductExistsTest
        {
            [Fact]
            public void Returns_False_When_ProductDoesNotExist()
            {
                // Arrange
                this.CommandMock.ExistingProduct.Returns(e => null);

                // Act
                var result = this.Validator.Validate(this.CommandMock);

                //Assert
                result.Should().BeFalse();
            }

            [Fact]
            public void Returns_True_When_ProductExists()
            {
                // Arrange
                this.CommandMock.ExistingProduct.Returns(e => new ProductViewModel());

                // Act
                var result = this.Validator.Validate(this.CommandMock);

                // Assert
                result.Should().BeTrue();
            }
        }
    }
}
