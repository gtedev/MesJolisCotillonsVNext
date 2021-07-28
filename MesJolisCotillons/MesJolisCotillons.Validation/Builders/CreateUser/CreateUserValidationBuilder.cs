namespace MesJolisCotillons.Validation.Builders.CreateUser
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands.User.Create;
    using MesJolisCotillons.Validation.Validators;
    using MesJolisCotillons.Validation.Validators.User;

    public class CreateUserValidationBuilder : ValidationBuilderBase<CreateUserCommand>, ICreateUserValidationBuilder
    {
        public CreateUserValidationBuilder(IValidationStepsBuilder<CreateUserCommand> builder)
            : base(builder)
        {
        }

        public override IEnumerable<IValidatorStep<CreateUserCommand>> Build()
        {
            return this.Builder
               .AddValidator<UserNotAlreadyExist>()
               .AddBreakIfNoValidStep()
               .AddValidator<PasswordIsLongEnough>()
               .AddBreakIfNoValidStep()
               .AddValidator<PasswordAreSame>()
               .Build();
        }
    }
}
