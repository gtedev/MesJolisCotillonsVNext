using FluentAssertions;
using MesJolisCotillons.Commands;
using MesJolisCotillons.Contracts;
using MesJolisCotillons.Validation.Validators;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Validation.UnitTests.Validators
{
    /// <summary>
    /// Validator Test base for validator that does not have constructor with parameters.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TValidator"></typeparam>
    public class ValidatorTestBase<TValidator, TCommand>
        where TValidator : IValidator<TCommand>, new()
        where TCommand : class, ICommand
    {
        protected TValidator Validator { get; }
        protected TCommand CommandMock { get; }

        public ValidatorTestBase()
        {
            this.CommandMock = Substitute.For<TCommand>();
            this.Validator = new TValidator();
        }

        public void AssertMessageFailureCode(MessageCode messageFailureCode)
        {
            this.Validator.MessageFailureCode.Should().Be(messageFailureCode);
        }
    }
}
