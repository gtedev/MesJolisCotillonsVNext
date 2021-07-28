namespace MesJolisCotillons.DataAccess.DataProviders
{
    using MesJolisCotillons.DataAccess.Entities.Context;

    public abstract class DataProviderBase<TContext> : IDataProvider
        where TContext : IDbContext
    {
        protected readonly TContext context;

        public DataProviderBase(TContext context)
        {
            this.context = context;
        }
    }
}
