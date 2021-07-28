namespace MesJolisCotillons.Core.Framework.Builders.Validation
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Validation.Validators;

    public interface IValidatorsProcessor<TCommand>
        where TCommand : ICommand
    {
        IValidationReport<TCommand> ProcessValidators(IEnumerable<IValidatorStep<TCommand>> validators, TCommand command);
    }
}
