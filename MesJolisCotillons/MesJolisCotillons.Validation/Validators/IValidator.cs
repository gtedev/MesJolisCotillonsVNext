namespace MesJolisCotillons.Validation.Validators
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts;

    public interface IValidator<in TCommand>
        where TCommand : ICommand
    {
        bool Validate(TCommand command);

        MessageCode MessageFailureCode { get; }
    }
}
