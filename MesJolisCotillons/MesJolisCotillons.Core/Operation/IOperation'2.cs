namespace MesJolisCotillons.Core
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Responses;

    public interface IOperation<TResponse>
    where TResponse : IResponse
    {
        Task<TResponse> Run();
    }
}
