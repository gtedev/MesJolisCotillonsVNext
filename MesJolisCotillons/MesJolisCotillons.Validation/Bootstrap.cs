namespace MesJolisCotillons.Validation
{
    using System.Reflection;
    using Autofac;
    using MesJolisCotillons.Helpers.Autofac;
    using MesJolisCotillons.Validation.Builders;
    using MesJolisCotillons.Validation.Validators;

    public static class Bootstrap
    {
        public static ContainerBuilder RegisterValidations(this ContainerBuilder container)
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
                .RegisterTypeGeneric(typeof(ValidationStepsBuilder<>));

            // validators
            container.RegisterAssemblyTypes(assembly)
                .InNamespace($"{ns}.{nameof(Validators)}")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return container;
        }
    }
}
