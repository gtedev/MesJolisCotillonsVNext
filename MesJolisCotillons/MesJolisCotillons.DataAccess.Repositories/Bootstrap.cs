namespace MesJolisCotillons.DataAccess.Repositories
{
    using Autofac;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;

    public static class Bootstrap
    {
        public static ContainerBuilder RegisterRepositories(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<ProductRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
