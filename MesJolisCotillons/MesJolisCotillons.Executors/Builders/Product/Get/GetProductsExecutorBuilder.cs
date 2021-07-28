namespace MesJolisCotillons.Executors.Builders.Product.Get
{
    using System.Collections.Generic;
    using MesJolisCotillons.Commands.Product.Get;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Executors.Product.Get;

    public class GetProductsExecutorBuilder : ExecutorBuilderBase<GetProductsCommand>, IGetProductsExecutorBuilder
    {
        private readonly IGetProductsExecutor getProductsExecutor;

        public GetProductsExecutorBuilder(
            IExecutorStepsBuilder<GetProductsCommand> builder,
            IGetProductsExecutor getProductsExecutor)
            : base(builder)
            => this.getProductsExecutor = getProductsExecutor;

        public override IEnumerable<IExecutorStep<GetProductsCommand>> Build(GetProductsCommand command)
        {
            return this.Builder
                .AddExecutor(this.getProductsExecutor)
                .Build();
        }
    }
}
