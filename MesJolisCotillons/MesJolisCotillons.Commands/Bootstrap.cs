namespace MesJolisCotillons.Commands
{
    using System.Reflection;
    using Autofac;

    public static class Bootstrap
    {
        public static ContainerBuilder RegisterCommands(this ContainerBuilder container)
        {
            var type = typeof(Bootstrap);
            var ns = type.Namespace;
            var assembly = typeof(Bootstrap).GetTypeInfo().Assembly;

            // builders
            container.RegisterAssemblyTypes(assembly)
                    .InNamespace($"{ns}.{nameof(Builders)}")
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            return container;
        }
    }
}
