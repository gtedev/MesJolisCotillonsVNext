using AutoMapper;

namespace MesJolisCotillons.DataAccess.Repositories.UnitTests.Repositories.User
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MesJolisCotillons.Common.UnitTests.MockMesJolisCotillonsContext;
    using MesJolisCotillons.Contracts.ViewModels.User;
    using MesJolisCotillons.DataAccess.Repositories.AutoMapper;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;
    using Xunit;
    using E = Entities.EntityModels;

    public class UserRepositoryTests
    {
        public abstract class UserRepositoryTest
        {
            public UserRepository UserRepository { get; set; }

            public MockMesJolisCotillonsContext MockMesJolisCotillonsContext { get; set; }

            public IMapper Mapper { get; set; }

            public UserRepositoryTest()
            {
                this.MockMesJolisCotillonsContext = new MockMesJolisCotillonsContext();

                // Arrange autoMapper
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<RepositoriesProfile>();
                });
                IMapper mapper = new Mapper(config);

                this.UserRepository = new UserRepository(this.MockMesJolisCotillonsContext, mapper);
            }

            protected void ArrangeDbContextWithTwoUsers()
            {
                var fakeUserOne = new E.User
                {
                    UserId = 1,
                    Email = "zinedine.zidane@gmail.com",
                    FirstName = "zinedine",
                    LastName = "zidane"

                };
                var fakeUserTwo = new E.User
                {
                    UserId = 2,
                    Email = "didier.deschamps@gmail.com",
                    FirstName = "didier",
                    LastName = "deschamps"
                };

                var listUser = new List<E.User>();
                listUser.Add(fakeUserOne);
                listUser.Add(fakeUserTwo);

                this.MockMesJolisCotillonsContext.Users.AddRange(listUser);
                this.MockMesJolisCotillonsContext.SaveChanges();
            }
        }

        public class FindUserShould : UserRepositoryTest
        {
            [Fact]
            public async Task ExpectedViewModelTypeAsync()
            {
                // Arrange
                this.ArrangeDbContextWithTwoUsers();

                // Act
                var user = await this.UserRepository.FindUser(1);

                // Assert
                user.Should().BeOfType<UserViewModel>();
                user.UserId.Should().Be(1);
            }

            [Fact]
            public async Task ExpectedUserDatasAsync()
            {
                // Arrange
                this.ArrangeDbContextWithTwoUsers();

                // Act
                var user = await this.UserRepository.FindUser(1);

                // Assert
                user.UserId.Should().Be(1);
                user.Email.Should().Be("zinedine.zidane@gmail.com");
                user.FirstName.Should().Be("zinedine");
                user.LastName.Should().Be("zidane");
            }
        }

        public class FindUserByEmailShould : UserRepositoryTest
        {
            [Fact]
            public async Task ExpectedViewModelTypeAsync()
            {
                // Arrange
                this.ArrangeDbContextWithTwoUsers();

                // Act
                var user = await this.UserRepository.FindUserByEmail("zinedine.zidane@gmail.com");

                // Assert
                user.Should().BeOfType<UserViewModel>();
                user.UserId.Should().Be(1);
            }

            [Fact]
            public async Task ExpectedUserDatasAsync()
            {
                // Arrange
                this.ArrangeDbContextWithTwoUsers();

                // Act
                var user = await this.UserRepository.FindUserByEmail("zinedine.zidane@gmail.com");

                // Assert
                user.UserId.Should().Be(1);
                user.Email.Should().Be("zinedine.zidane@gmail.com");
                user.FirstName.Should().Be("zinedine");
                user.LastName.Should().Be("zidane");
            }
        }

        public class CreateUserShould : UserRepositoryTest
        {
            [Fact]
            public async Task BeAdded_In_Context_WithExpectedData()
            {
                // Arrange
                var email = "zinedine.zidane@gmail.com";
                var firstName = "zinedine";
                var lastName = "zidane";

                // Act
                await this.UserRepository.CreateUser(
                    email,
                    firstName,
                    lastName);

                this.MockMesJolisCotillonsContext.SaveChanges();

                // Assert
                var userOne = this.MockMesJolisCotillonsContext.Users.FirstOrDefault();

                this.MockMesJolisCotillonsContext.Users.Should().HaveCount(1);

                userOne.UserId.Should().Be(1);
                userOne.Email.Should().Be(email);
                userOne.FirstName.Should().Be(firstName);
                userOne.LastName.Should().Be(lastName);
            }
        }
    }
}
