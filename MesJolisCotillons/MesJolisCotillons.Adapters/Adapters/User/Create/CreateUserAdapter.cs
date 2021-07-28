namespace MesJolisCotillons.Adapters.User.Create
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests.User.Create;
    using MesJolisCotillons.Contracts.ViewModels.User;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;

    public class CreateUserAdapter : AdapterBase<CreateUserRequest>, ICreateUserAdapter
    {
        private IUserRepository userRepository;

        public CreateUserAdapter(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public UserViewModel ExistingUser { get; set; }

        public override async Task InitAdapter(CreateUserRequest request)
        {
            this.ExistingUser = await this.userRepository.FindUserByEmail(request.Email);
        }
    }
}
