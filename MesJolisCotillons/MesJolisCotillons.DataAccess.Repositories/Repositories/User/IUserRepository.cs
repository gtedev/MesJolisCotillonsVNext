namespace MesJolisCotillons.DataAccess.Repositories.Repositories
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.ViewModels.User;

    public interface IUserRepository: IRepositoryBase
    {
        Task<UserViewModel> FindUser(int userId);

        Task<UserViewModel> FindUserByEmail(string email);

        Task CreateUser(string email, string firstName, string lastName);
    }
}
