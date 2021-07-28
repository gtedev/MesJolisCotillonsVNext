namespace MesJolisCotillons.Adapters.User.Create
{
    using MesJolisCotillons.Contracts.Requests.User.Create;
    using MesJolisCotillons.Contracts.ViewModels.User;

    public interface ICreateUserAdapter : IAdapter<CreateUserRequest>
    {
        UserViewModel ExistingUser { get; }
    }
}
