using FluentAssertions;
using MesJolisCotillons.Commands.Product;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Validation.Validators.Product;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Validation.UnitTests.Validators.Product
{
    public class PageParametersGreaterThanOneTests : ValidatorTestBase<PageParametersGreaterThanOne, IProductsPagedCommand>
    {
        public abstract class PageParametersGreaterThanOneTest : ValidatorTestBase<PageParametersGreaterThanOne, IProductsPagedCommand>
        {
        }

        public class MessageCodeShould : PageParametersGreaterThanOneTest
        {
            [Fact]
            public void BeCorrect() => this.AssertMessageFailureCode(MessageCode.PageParametersNotGreaterThanZero);
        }

        public class ValidateShould : PageParametersGreaterThanOneTest
        {
            [Fact]
            public void Returns_False_When_Page_Equals_Zero()
            {
                // Arrange
                this.CommandMock.Page.Returns(0);

                // Act
                var result = this.Validator.Validate(this.CommandMock);

                //Assert
                result.Should().BeFalse();
            }

            [Fact]
            public void Returns_False_When_PageNumber_Equals_Zero()
            {
                // Arrange
                this.CommandMock.PageSize.Returns(0);

                // Act
                var result = this.Validator.Validate(this.CommandMock);

                //Assert
                result.Should().BeFalse();
            }

            [Fact]
            public void Returns_True_When_Page_And_PageNumber_Are_Greather_Than_One()
            {
                // Arrange
                this.CommandMock.PageSize.Returns(2);
                this.CommandMock.Page.Returns(1);

                // Act
                var result = this.Validator.Validate(this.CommandMock);

                // Assert
                result.Should().BeTrue();
            }
        }
    }
}
