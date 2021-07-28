namespace MesJolisCotillons.DataAccess.Repositories.AutoMapper
{
    using global::AutoMapper;
    using MesJolisCotillons.Contracts.ViewModels.Product;
    using MesJolisCotillons.Contracts.ViewModels.User;
    using MesJolisCotillons.DataAccess.Entities.EntityModels;

    public class RepositoriesProfile : Profile
    {
        public RepositoriesProfile()
        {
            this.CreateMap<User, UserViewModel>();
            this.CreateMap<Product, ProductViewModel>();
            this.CreateMap<Category, CategoryViewModel>();
        }
    }
}
