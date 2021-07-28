namespace MesJolisCotillons.VNext.DI.Autofac.Extensions
{
    using global::Autofac;
    using MesJolisCotillons.Resources.ResourceManagerResolvers;
    using MesJolisCotillons.Resources.Services;

    public static class Bootstrap
    {
        public static ContainerBuilder RegisterResources(this ContainerBuilder containerBuilder)
        {
            //// Taking IResourceManagerResolver just to get the Assembly here

            var resourceAssembly = typeof(IResourceManagerResolver).Assembly;
            var resourceResolverNameSpace = typeof(IResourceManagerResolver).Namespace;

            containerBuilder.RegisterAssemblyTypes(resourceAssembly)
                   .Where(t => t.Namespace.Contains(resourceResolverNameSpace))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            containerBuilder.RegisterType<ResourceLocalizerService>()
                   .As<IResourceLocalizerService>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            containerBuilder.RegisterType<MessagesLocalizerService>()
                   .As<IMessagesLocalizerService>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
