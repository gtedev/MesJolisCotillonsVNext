namespace MesJolisCotillons.Executors.Builders.Product.Create
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands.Product.Create;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Executors.Product.Create;

    public class CreateProductExecutorBuilder : ExecutorBuilderBase<CreateProductCommand>, ICreateProductExecutorBuilder
    {
        private readonly ICreateProductExecutor createProductExecutor;

        public CreateProductExecutorBuilder(
            IExecutorStepsBuilder<CreateProductCommand> builder,
            ICreateProductExecutor createProductExecutor)
            : base(builder)
        {
            this.createProductExecutor = createProductExecutor;
        }

        public override IEnumerable<IExecutorStep<CreateProductCommand>> Build(CreateProductCommand command)
        {
            return this.Builder
                .AddExecutor(this.createProductExecutor)
                .AddSaveChangesStep()
                .Build();
        }
    }
}
