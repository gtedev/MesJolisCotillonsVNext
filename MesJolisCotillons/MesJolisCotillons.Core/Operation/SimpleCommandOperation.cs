namespace MesJolisCotillons.Core
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.DataAccess.Entities.Context;

    public abstract class SimpleCommandOperation<TRequest, TResponse> : IOperation<TRequest, TResponse>
        where TRequest : IRequest
        where TResponse : IResponse
    {
        private readonly ISaveDbContext saveDbContext;

        public SimpleCommandOperation(ISaveDbContext saveDbContext)
        {
            this.saveDbContext = saveDbContext;
        }

        protected abstract Task RunCommandOperation(TRequest request);

        protected abstract Task<TResponse> BuildResponse(TRequest request);

        public async Task<TResponse> Run(TRequest request)
        {
            await this.RunCommandOperation(request);

            //// We assume that the SaveChangesAsync below is the only one done
            /// for whe whole worflow that RunCommandOperation initiates (perRequest logic).
            await this.saveDbContext.SaveChangesAsync();

            var response = await this.BuildResponse(request);

            return response;
        }
    }
}
