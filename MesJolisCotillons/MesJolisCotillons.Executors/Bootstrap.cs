namespace MesJolisCotillons.Executors
{
    using Autofac;
    using MesJolisCotillons.Executors.Builder;
    using MesJolisCotillons.Helpers.Autofac;
    using System.Reflection;

    public static class Bootstrap
    {
        public static ContainerBuilder RegisterExecutors(this ContainerBuilder container)
        {
            var type = typeof(Bootstrap);
            var ns = type.Namespace;
            var assembly = typeof(Bootstrap).GetTypeInfo().Assembly;

            // builders
            container.RegisterAssemblyTypes(assembly)
                .InNamespace($"{ns}.{nameof(Builders)}")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            container
                .RegisterTypeGeneric(typeof(ExecutorStepsBuilder<>))
                .RegisterTypeGeneric(typeof(SaveChangesExecutor<>));

            // executors
            container.RegisterAssemblyTypes(assembly)
                    .InNamespace($"{ns}")
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            return container;
        }
    }
}
