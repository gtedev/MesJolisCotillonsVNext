namespace MesJolisCotillons.Adapters
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests;

    public abstract class AdapterBase<TRequest> : IAdapter<TRequest>
        where TRequest : IRequest
    {
        public AdapterBase()
        {
        }

        public async Task Init(TRequest request)
        {
            await this.InitAdapter(request);
        }

        public abstract Task InitAdapter(TRequest request);
    }
}
