namespace MesJolisCotillons.Commands.Builders.User.Create
{
    using MesJolisCotillons.Adapters.User.Create;
    using MesJolisCotillons.Commands.User.Create;
    using MesJolisCotillons.Contracts.Requests.User.Create;

    public class CreateUserCommandBuilder : ICreateUserCommandBuilder
    {
        public CreateUserCommand Build(ICreateUserAdapter adapter, CreateUserRequest request)
        {
            return new CreateUserCommand
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                ConfirmedPassword = request.ConfirmedPassword,
                ExistingUser = adapter.ExistingUser,
                MinimumPasswordCharacterNumber = 6// TODO: put this in a Constants file
            };
        }
    }
}
