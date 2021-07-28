namespace MesJolisCotillons.Response.Builders.User.Create
{
    using MesJolisCotillons.Commands.User.Create;
    using MesJolisCotillons.Contracts.Responses.User.Create;

    public interface ICreateUserResponseBuilder : IResponseBuilder<CreateUserCommand, CreateUserResponse>
    {
    }
}
