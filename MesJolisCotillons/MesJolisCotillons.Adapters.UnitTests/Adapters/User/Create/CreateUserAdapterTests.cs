using FluentAssertions;
using MesJolisCotillons.Adapters.User.Create;
using MesJolisCotillons.Contracts.Requests.User.Create;
using MesJolisCotillons.Contracts.ViewModels.Product;
using MesJolisCotillons.Contracts.ViewModels.User;
using MesJolisCotillons.DataAccess.Repositories.Repositories;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Adapters.UnitTests.Adapters.User.Create
{
    public class CreateUserAdapterTests
    {
        public abstract class CreateUserAdapterTest : AdapterTestBase<CreateUserAdapter, CreateUserRequest>
        {
            protected IUserRepository UserRepositoryMock;

            public CreateUserAdapterTest()
            {
                this.UserRepositoryMock = Substitute.For<IUserRepository>();

                this.Adapter = new CreateUserAdapter(this.UserRepositoryMock);
            }
        }

        public class ExistingUserShould : CreateUserAdapterTest
        {
            [Fact]
            public async Task BeSet_WhenUserExistsAsync()
            {
                // Arrange
                this.UserRepositoryMock
                    .FindUserByEmail(Arg.Any<string>())
                    .Returns(new UserViewModel());

                // Act
                await this.Adapter.InitAdapter(this.Request);

                // Assert
                this.Adapter
                    .ExistingUser
                    .Should().NotBeNull();
            }

            [Fact]
            public async Task BeSet_WhenUserDoesNotExistsAsync()
            {
                // Arrange
                this.UserRepositoryMock
                    .FindUserByEmail(Arg.Any<string>())
                    .Returns((UserViewModel)null);

                // Act
                await this.Adapter.InitAdapter(this.Request);

                // Assert
                this.Adapter
                    .ExistingUser
                    .Should().BeNull();
            }

            [Fact]
            public async Task Call_FindProduct_In_ProductRepositoryAsync()
            {
                // Arrange
                this.UserRepositoryMock
                    .FindUserByEmail(Arg.Any<string>())
                    .Returns((UserViewModel)null);

                // Act
                await this.Adapter.InitAdapter(this.Request);

                // Assert
                await this.UserRepositoryMock
                   .Received(1)
                   .FindUserByEmail(Arg.Any<string>());
            }
        }
    }
}
