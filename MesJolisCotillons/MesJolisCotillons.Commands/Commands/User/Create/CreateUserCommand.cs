namespace MesJolisCotillons.Commands.User.Create
{
    using MesJolisCotillons.Commands.Commands.User;
    using MesJolisCotillons.Contracts.ViewModels.User;

    public class CreateUserCommand : ICommand,
        IExistingUserCommand,
        IUserPasswordCommand
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string ConfirmedPassword { get; set; }

        public int MinimumPasswordCharacterNumber { get; set; }

        public UserViewModel ExistingUser { get; set; }
    }
}
