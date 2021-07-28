namespace MesJolisCotillons.Core.Framework.Builders.Runner
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;

    public interface IRunnerOperationStep<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : IResponse
    {
        /// <summary>
        /// Run the running operation function, it returns an operation response
        /// </summary>
        /// <returns></returns>
        Task<TResponse> Run();
    }
}
