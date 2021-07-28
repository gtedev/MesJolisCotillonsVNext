namespace MesJolisCotillons.Adapters
{
    using Autofac;
    using System.Reflection;

    public static class Bootstrap
    {
        public static ContainerBuilder RegisterAdapters(this ContainerBuilder container)
        {
            var type = typeof(Bootstrap);
            var ns = type.Namespace;
            var assembly = typeof(Bootstrap).GetTypeInfo().Assembly;

            var nameSpace = $"{ns}.{nameof(Adapters)}";

            container.RegisterAssemblyTypes(assembly)
                    .InNamespace($"{ns}")
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            return container;
        }
    }
}
