namespace MesJolisCotillons.Commands.Commands.User
{
    using MesJolisCotillons.Contracts.ViewModels.User;

    public interface IExistingUserCommand : ICommand
    {
        UserViewModel ExistingUser { get; }
    }
}
