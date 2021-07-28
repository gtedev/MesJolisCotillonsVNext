namespace MesJolisCotillons.Executors.User.Create
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands.User.Create;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;

    public class CreateUserExecutor : ICreateUserExecutor
    {
        private readonly IUserRepository userRepository;

        public CreateUserExecutor(IUserRepository userRepository)
        => this.userRepository = userRepository;

        public async Task ExecuteAsync(CreateUserCommand command)
        {
            //// adding a user, might need at some point to deal with password.

            await this.userRepository.CreateUser(
                command.Email,
                command.FirstName,
                command.LastName);
        }
    }
}
