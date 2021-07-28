using FluentAssertions;
using MesJolisCotillons.Commands.Builders.TemplateNamespace;
using MesJolisCotillons.Commands.TemplateNamespace;
using MesJolisCotillons.Contracts.Requests.TemplateNamespace;
using NSubstitute;
using Xunit;

namespace MesJolisCotillons.Commands.UnitTests.Builders.TemplateNamespace
{
    public class TemplateOperationNameCommandBuilderUnitTests
    {
        public abstract class TemplateOperationNameCommandBuilderUnitTest
            : CommandBuilderTestBase<TemplateOperationNameRequest, TemplateOperationNameCommand, TemplateOperationNameCommandBuilder>
        {
            public TemplateOperationNameCommandBuilderUnitTest() : base()
            {
                this.CommandBuilder = new TemplateOperationNameCommandBuilder();
            }
        }

        public class BuildShould : TemplateOperationNameCommandBuilderUnitTest
        {
            [Fact]
            public void Returns_Command_NonNull()
            {
                // Arrange
                //var user = new UserViewModel();
                //this.Adapter.ExistingUser.Returns(user);

                //this.Request = new TemplateRequest
                //{
                //    Email = "bruce.lee@gmail.com",
                //    FirstName = "Bruce",
                //};

                // Act
                this.Command = this.CommandBuilder.Build(this.Request);

                // Assert
                this.Command.Should().NotBeNull();
            }

            [Fact]
            public void Returns_Command_withPropertiesSet()
            {
                // Arrange
                //var user = new UserViewModel();
                //this.Adapter.ExistingUser.Returns(user);

                //this.Request = new TemplateRequest
                //{
                //    Email = "bruce.lee@gmail.com",
                //    FirstName = "Bruce",
                //};

                // Act
                this.Command = this.CommandBuilder.Build(this.Request);

                // Assert
                //this.Command.Email.Should().Be("bruce.lee@gmail.com");
            }
        }
    }
}
