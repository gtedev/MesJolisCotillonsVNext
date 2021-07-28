namespace MesJolisCotillons.DataAccess.Entities.Context
{
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;

    public interface IUserDbContext : IDbContext
    {
        DbSet<User> Users { get; }

        DbSet<AdminUser> Admin_Users { get; }

        DbSet<CustomerUser> Customer_Users { get; }
    }
}
