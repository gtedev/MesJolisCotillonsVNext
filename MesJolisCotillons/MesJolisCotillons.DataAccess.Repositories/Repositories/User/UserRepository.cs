namespace MesJolisCotillons.DataAccess.Repositories.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using global::AutoMapper;
    using MesJolisCotillons.Contracts.ViewModels.User;
    using MesJolisCotillons.DataAccess.Entities.Context;
    using Microsoft.EntityFrameworkCore;
    using E = Entities.EntityModels;

    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IMesJolisCotillonsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public Task CreateUser(string email, string firstName, string lastName)
        {
            var user = new E.User
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            return this.context.Users.AddAsync(user);
        }

        public Task<UserViewModel> FindUser(int userId)
        {
            this.context.ProductCategories
                .Where(p => p.ProductFk > 0)
                .Select(p => new
                {
                    Toto = p.ProductFkNavigation.Name
                }).ToList();

            return this.context.Users
               .Where(item => item.UserId == userId)
               .Select(item => this.mapper.Map<UserViewModel>(item))
               .FirstOrDefaultAsync();
        }

        public Task<UserViewModel> FindUserByEmail(string email)
        {
            return this.context.Users
                .Where(item => item.Email == email)
                .Select(item => this.mapper.Map<UserViewModel>(item))
                .FirstOrDefaultAsync();
        }
    }
}
