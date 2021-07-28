namespace MesJolisCotillons.Operations
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using MesJolisCotillons.Core;

    public static class Bootstrap
    {
        public static ContainerBuilder RegisterOperations(this ContainerBuilder container)
        {
            var type = typeof(Bootstrap);
            var ns = type.Namespace;
            var assembly = typeof(Bootstrap).GetTypeInfo().Assembly;

            container.RegisterAssemblyTypes(assembly)
                .InstancePerLifetimeScope()
                .AsClosedTypesOf(typeof(IOperation<,>));

            container.RegisterAssemblyTypes(assembly)
                .InstancePerLifetimeScope()
                .AsClosedTypesOf(typeof(IOperation<>));

            return container;
        }
    }
}
