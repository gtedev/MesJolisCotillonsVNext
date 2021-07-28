namespace MesJolisCotillons.Validation.Builders
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Validation.Validators;

    public interface IValidationBuilder<TCommand>
        where TCommand : ICommand
    {
        IEnumerable<IValidatorStep<TCommand>> Build();
    }
}
