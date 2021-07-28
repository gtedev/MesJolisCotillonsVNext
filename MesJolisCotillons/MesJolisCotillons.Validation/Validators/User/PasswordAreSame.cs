namespace MesJolisCotillons.Validation.Validators.User
{
    using MesJolisCotillons.Commands.Commands.User;
    using MesJolisCotillons.Contracts;

    public class PasswordAreSame : IValidator<IUserPasswordCommand>
    {
        public MessageCode MessageFailureCode => MessageCode.PasswordAreNotSame;

        public bool Validate(IUserPasswordCommand command)
            => command.Password == command.ConfirmedPassword;
    }
}
