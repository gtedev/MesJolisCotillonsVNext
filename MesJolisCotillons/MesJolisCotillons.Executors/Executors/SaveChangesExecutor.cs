namespace MesJolisCotillons.Executors
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.DataAccess.Entities.Context;

    public class SaveChangesExecutor<TCommand> : ISaveChangesExecutor<TCommand>
        where TCommand : ICommand
    {
        private readonly ISaveDbContext saveDbContext;

        public SaveChangesExecutor(ISaveDbContext saveDbContext)
        => this.saveDbContext = saveDbContext;

        public async Task ExecuteAsync(TCommand command)
        {
            await this.saveDbContext.SaveChangesAsync();
        }
    }
}
