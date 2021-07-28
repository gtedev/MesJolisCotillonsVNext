namespace MesJolisCotillons.Validation.Validators.User
{
    using MesJolisCotillons.Commands.Commands.User;
    using MesJolisCotillons.Contracts;

    public class PasswordIsLongEnough : IValidator<IUserPasswordCommand>
    {
        public MessageCode MessageFailureCode => MessageCode.PasswordIsNotLongEnough;

        public bool Validate(IUserPasswordCommand command)
            => command.Password.Length >= 6 && command.ConfirmedPassword.Length >= 6;
    }
}
