using FluentAssertions;
using MesJolisCotillons.Adapters.TemplateNamespace;
using MesJolisCotillons.Contracts.Requests.TemplateNamespace;
using MesJolisCotillons.DataAccess.Repositories.Repositories;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace MesJolisCotillons.Adapters.UnitTests.Adapters.TemplateNamespace
{
    public class TemplateOperationNameAdapterTests
    {
        public abstract class TemplateOperationNameAdapterTest : AdapterTestBase<TemplateOperationNameAdapter, TemplateOperationNameRequest>
        {
            //protected IUserRepository UserRepositoryMock;

            public TemplateOperationNameAdapterTest()
            {
                //this.UserRepositoryMock = Substitute.For<IUserRepository>();

                this.Adapter = new TemplateOperationNameAdapter(this.UserRepositoryMock);
            }
        }

        public class ExistingUserShould : TemplateOperationNameAdapterTest
        {
            [Fact]
            public async Task BeSet_WhenUserExistsAsync()
            {
                // Arrange
                //this.UserRepositoryMock
                //    .FindUserByEmail(Arg.Any<string>())
                //    .Returns(new UserViewModel());

                // Act
                await this.Adapter.InitAdapter(this.Request);

                // Assert
                //this.Adapter
                //    .ExistingUser
                //    .Should().NotBeNull();
            }
        }
    }
}
