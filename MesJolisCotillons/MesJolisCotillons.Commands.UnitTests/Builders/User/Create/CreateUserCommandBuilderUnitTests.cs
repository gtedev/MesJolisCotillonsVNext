using FluentAssertions;
using MesJolisCotillons.Adapters.User.Create;
using MesJolisCotillons.Commands.Builders.User.Create;
using MesJolisCotillons.Commands.User.Create;
using MesJolisCotillons.Contracts.Requests.User.Create;
using MesJolisCotillons.Contracts.ViewModels.User;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Commands.UnitTests.Builders.User.Create
{
    public class CreateUserCommandBuilderUnitTests
    {
        public abstract class CreateUserCommandBuilderUnitTest
            : CommandBuilderTestBase<ICreateUserAdapter, CreateUserRequest, CreateUserCommand, CreateUserCommandBuilder>
        {
            public CreateUserCommandBuilderUnitTest() : base()
            {
                this.CommandBuilder = new CreateUserCommandBuilder();
            }
        }

        public class BuildShould : CreateUserCommandBuilderUnitTest
        {
            [Fact]
            public void Returns_Command_NonNull()
            {
                // Arrange
                var user = new UserViewModel();
                this.Adapter.ExistingUser.Returns(user);

                this.Request = new CreateUserRequest
                {
                    Email = "bruce.lee@gmail.com",
                    FirstName = "Bruce",
                    LastName = "Lee",
                    Password = "123456",
                    ConfirmedPassword = "123456"
                };

                // Act
                this.Command = this.CommandBuilder.Build(this.Adapter, this.Request);

                // Assert
                this.Command.Should().NotBeNull();
            }

            [Fact]
            public void Returns_Command_withPropertiesSet()
            {
                // Arrange
                var user = new UserViewModel();
                this.Adapter.ExistingUser.Returns(user);

                this.Request = new CreateUserRequest
                {
                    Email = "bruce.lee@gmail.com",
                    FirstName = "Bruce",
                    LastName = "Lee",
                    Password = "123456",
                    ConfirmedPassword = "123456"
                };

                // Act
                this.Command = this.CommandBuilder.Build(this.Adapter, this.Request);

                // Assert
                this.Command.Email.Should().Be("bruce.lee@gmail.com");
                this.Command.FirstName.Should().Be("Bruce");
                this.Command.LastName.Should().Be("Lee");
                this.Command.Password.Should().Be("123456");
                this.Command.ConfirmedPassword.Should().Be("123456");
            }
        }
    }
}
