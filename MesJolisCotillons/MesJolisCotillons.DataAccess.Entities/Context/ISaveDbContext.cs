namespace MesJolisCotillons.DataAccess.Entities.Context
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.Storage;

    public interface ISaveDbContext : IDbContext
    {
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        IDbContextTransaction BeginTransaction();
    }
}
