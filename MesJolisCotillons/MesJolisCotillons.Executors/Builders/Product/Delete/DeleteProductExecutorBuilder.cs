namespace MesJolisCotillons.Executors.Builders.Product.Delete
{
    using MesJolisCotillons.Commands.Product.Delete;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Executors.Product.Delete;
    using System.Collections.Generic;

    public class DeleteProductExecutorBuilder : ExecutorBuilderBase<DeleteProductCommand>, IDeleteProductExecutorBuilder
    {
        private readonly IDeleteProductExecutor deleteProductExecutor;

        public DeleteProductExecutorBuilder(
            IExecutorStepsBuilder<DeleteProductCommand> builder,
            IDeleteProductExecutor deleteProductExecutor)
            : base(builder)
        {
            this.deleteProductExecutor = deleteProductExecutor;
        }

        public override IEnumerable<IExecutorStep<DeleteProductCommand>> Build(DeleteProductCommand command)
        {
            return this.Builder
                .AddExecutor(this.deleteProductExecutor)
                .AddSaveChangesStep()
                .Build();
        }
    }
}
