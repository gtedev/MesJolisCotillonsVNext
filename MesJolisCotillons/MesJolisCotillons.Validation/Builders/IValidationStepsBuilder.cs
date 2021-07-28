namespace MesJolisCotillons.Validation.Builders
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Validation.Validators;

    public interface IValidationStepsBuilder<TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        /// Add a validation step in the pipeline.
        /// </summary>
        /// <typeparam name="TValidator"></typeparam>
        /// <returns>validation builder.</returns>
        IValidationStepsBuilder<TCommand> AddValidator<TValidator>()
         where TValidator : IValidator<TCommand>, new();

        /// <summary>
        /// Use this method to add a step that will avoid going ahead in Validation if previous validation fails.
        /// </summary>
        /// <returns>validation builder.</returns>
        IValidationStepsBuilder<TCommand> AddBreakIfNoValidStep();

        /// <summary>
        /// Build the list of validators to run.
        /// </summary>
        /// <returns>.</returns>
        IEnumerable<IValidatorStep<TCommand>> Build();
    }
}
