namespace MesJolisCotillons.Commands.Commands.User
{
    public interface IUserPasswordCommand : ICommand
    {
        string Password { get; }

        string ConfirmedPassword { get; }

        int MinimumPasswordCharacterNumber { get; }
    }
}
