namespace MesJolisCotillons.DataAccess.DataProviders
{
    using System.Linq;
    using MesJolisCotillons.DataAccess.Entities.Context;
    using MesJolisCotillons.DataAccess.Entities.EntityModels;

    public class UserDataProvider : DataProviderBase<IUserDbContext>, IUserDataProvider
    {
        public UserDataProvider(IUserDbContext context)
            : base(context)
        {
        }

        public IQueryable<AdminUser> FindAllAdminUser()
        {
            return this.context.Admin_Users;
        }

        public IQueryable<CustomerUser> FindAllCustomerUser()
        {
            return this.context.Customer_Users;
        }

        public IQueryable<User> FindAllUser()
        {
            return this.context.Users;
        }
    }
}
