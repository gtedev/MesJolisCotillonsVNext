namespace MesJolisCotillons.Executors.Builders.User.Create
{
    using MesJolisCotillons.Commands.User.Create;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Executors.User.Create;
    using System.Collections.Generic;

    public class CreateUserExecutorBuilder : ExecutorBuilderBase<CreateUserCommand>, ICreateUserExecutorBuilder
    {
        private readonly ICreateUserExecutor createUserExecutor;

        public CreateUserExecutorBuilder(
            IExecutorStepsBuilder<CreateUserCommand> builder,
            ICreateUserExecutor createUserExecutor)
            : base(builder)
        {
            this.createUserExecutor = createUserExecutor;
        }

        public override IEnumerable<IExecutorStep<CreateUserCommand>> Build(CreateUserCommand command)
        {
            return this.Builder
                .AddExecutor(this.createUserExecutor)
                .AddSaveChangesStep()
                .Build();
        }
    }
}
