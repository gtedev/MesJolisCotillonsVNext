namespace MesJolisCotillons.Adapters
{
    using Autofac;
    using MesJolisCotillons.Helpers.Autofac;
    using MesJolisCotillons.Response.Builders;
    using MesJolisCotillons.Response.Builders.Default;
    using System.Reflection;

    public static class Bootstrap
    {
        public static ContainerBuilder RegisterResponse(this ContainerBuilder container)
        {
            var type = typeof(Bootstrap);
            var ns = type.Namespace;
            var assembly = typeof(Bootstrap).GetTypeInfo().Assembly;

            container.RegisterAssemblyTypes(assembly)
                    .InNamespace($"{ns}.{nameof(Response)}")
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            var responseBuilderAssembly = typeof(ICustomInvalidRequestResponseBuilder).Assembly;
            var responseBuilderNameSpace = typeof(ICustomInvalidRequestResponseBuilder).Namespace;

            container.RegisterAssemblyTypes(responseBuilderAssembly)
                   .Where(t => t.Namespace.Contains(responseBuilderNameSpace))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            container
                .RegisterTypeGeneric(typeof(DefaultResponseBuilder<,>))
                .RegisterTypeGeneric(typeof(FailureResponseBuilder<,>));

            return container;
        }
    }
}
