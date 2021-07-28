namespace MesJolisCotillons.Executors
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;

    public interface IExecutor<in TCommand>
        where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }
}
