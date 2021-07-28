namespace MesJolisCotillons.Adapters
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests;

    public interface IAdapter<TRequest> : IAdapter
        where TRequest : IRequest
    {
        Task Init(TRequest request);
    }
}
