namespace MesJolisCotillons.DataAccess.Repositories.Repositories
{
    using global::AutoMapper;
    using MesJolisCotillons.DataAccess.Entities.Context;

    public abstract class RepositoryBase
    {
        protected readonly IMesJolisCotillonsContext context;
        protected readonly IMapper mapper;

        public RepositoryBase(IMesJolisCotillonsContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
    }
}
