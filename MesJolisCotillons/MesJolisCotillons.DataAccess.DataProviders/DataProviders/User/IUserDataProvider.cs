namespace MesJolisCotillons.DataAccess.DataProviders
{
    using System.Linq;
    using MesJolisCotillons.DataAccess.Entities.EntityModels;

    public interface IUserDataProvider : IDataProvider
    {
        IQueryable<User> FindAllUser();

        IQueryable<AdminUser> FindAllAdminUser();

        IQueryable<CustomerUser> FindAllCustomerUser();
    }
}
