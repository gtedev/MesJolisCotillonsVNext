using MesJolisCotillons.Commands.User.Create;
using MesJolisCotillons.DataAccess.Repositories.Repositories;
using MesJolisCotillons.Executors.User.Create;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Executors.UnitTests.User.Create
{
    public class CreateUserExecutorUnitTests
    {
        public abstract class CreateUserExecutorUnitTest
            : ExecutorTestBase<CreateUserCommand, IUserRepository, CreateUserExecutor>
        {
            public CreateUserExecutorUnitTest() : base()
            {
                this.Executor = new CreateUserExecutor(this.RepositoryMock);
            }
        }

        public class ExecuteShould : CreateUserExecutorUnitTest
        {
            [Fact]
            public async Task Call_CreateUser_From_Repository_WithExpectedPropertiesAsync()
            {
                // Arrange
                this.Command = new CreateUserCommand
                {
                    Email = "bruce.lee@gmail.com",
                    FirstName = "Bruce",
                    LastName = "Lee",
                    Password = "123456",
                    ConfirmedPassword = "123456"
                };

                // Act
                await this.Executor.ExecuteAsync(this.Command);

                // Assert
                await this.RepositoryMock
                    .Received()
                    .CreateUser(
                        Arg.Is<string>(email => email == "bruce.lee@gmail.com"),
                        Arg.Is<string>(name => name == "Bruce"),
                        Arg.Is<string>(lastName => lastName == "Lee"));
            }
        }
    }
}
