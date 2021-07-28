namespace MesJolisCotillons.Commands.Builders.User.Create
{
    using MesJolisCotillons.Adapters.User.Create;
    using MesJolisCotillons.Commands.User.Create;
    using MesJolisCotillons.Contracts.Requests.User.Create;

    public interface ICreateUserCommandBuilder : ICommandBuilder<CreateUserCommand, CreateUserRequest, ICreateUserAdapter>
    {
    }
}
