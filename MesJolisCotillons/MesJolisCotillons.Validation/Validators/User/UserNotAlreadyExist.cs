namespace MesJolisCotillons.Validation.Validators.User
{
    using MesJolisCotillons.Commands.Commands.User;
    using MesJolisCotillons.Contracts;

    class UserNotAlreadyExist : IValidator<IExistingUserCommand>
    {
        public MessageCode MessageFailureCode => MessageCode.UserAlreadyExists;

        public bool Validate(IExistingUserCommand command)
            => command.ExistingUser == null;
    }
}
