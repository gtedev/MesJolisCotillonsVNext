using FluentAssertions;
using MesJolisCotillons.Commands.Product;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Validation.Validators.Product;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace MesJolisCotillons.Validation.UnitTests.Validators.Product
{
    public class CategoriesExistTests : ValidatorTestBase<CategoriesExist, IExistingCategoriesCommand>
    {
        public abstract class CategoriesExistTest : ValidatorTestBase<CategoriesExist, IExistingCategoriesCommand>
        {
        }

        public class MessageCodeShould : CategoriesExistTest
        {
            [Fact]
            public void BeCorrect() => this.AssertMessageFailureCode(MessageCode.CategoriesDoNotExist);
        }

        public class ValidateShould : CategoriesExistTest
        {
            [Fact]
            public void Returns_False_When_NonExistingCategories_CountGreaterThanOne()
            {
                // Arrange
                this.CommandMock.NonExistingCategories
                    .Returns(new List<int> { 1 });

                // Act
                var result = this.Validator.Validate(this.CommandMock);

                //Assert
                result.Should().BeFalse();
            }

            [Fact]
            public void Returns_True_When_NonExistingCategories_Count_Equals_Zero()
            {
                // Arrange
                this.CommandMock.NonExistingCategories
                    .Returns(new List<int>());

                // Act
                var result = this.Validator.Validate(this.CommandMock);

                // Assert
                result.Should().BeTrue();
            }
        }
    }
}
