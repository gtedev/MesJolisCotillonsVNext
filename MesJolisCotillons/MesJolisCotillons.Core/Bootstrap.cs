namespace MesJolisCotillons.Core
{
    using System.Reflection;
    using Autofac;
    using MesJolisCotillons.Core.Framework.Builders.Executor;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Core.Framework.Builders.Validation;
    using MesJolisCotillons.Core.Framework.Factories;
    using MesJolisCotillons.Helpers.Autofac;

    public static class Bootstrap
    {
        public static ContainerBuilder RegisterCore(this ContainerBuilder container)
        {
            var type = typeof(Bootstrap);
            var ns = type.Namespace;
            var assembly = typeof(Bootstrap).GetTypeInfo().Assembly;

            // operations builders
            container
                .RegisterTypeGeneric(typeof(OperationBuilder<,,>))
                .RegisterTypeGeneric(typeof(OperationBuilderFactory<,>))
                .RegisterTypeGeneric(typeof(CommandBuilderStepFactory<,,,>))
                .RegisterTypeGeneric(typeof(OperationBuilderContext<,>))
                .RegisterTypeGeneric(typeof(ValidationBuilderStepFactory<,>))
                .RegisterTypeGeneric(typeof(ExecutorBuilderStepFactory<,>))
                .RegisterTypeGeneric(typeof(ResponseBuilderStepFactory<,>))
                .RegisterTypeGeneric(typeof(ValidatorsProcessor<>))
                .RegisterTypeGeneric(typeof(ExecutorsProcessor<>))
                .RegisterTypeGeneric(typeof(OperationBuilderStepFactory<,>))
                .RegisterTypeGeneric(typeof(RunnerOperationStepFactory<,>))
                .RegisterTypeGeneric(typeof(ProcessOperationFactory<,>))
                .RegisterTypeGeneric(typeof(CommandGeneratorFactory<,>));

            container.RegisterAssemblyTypes(assembly)
                    .InNamespace($"{ns}.{nameof(Framework.Builders)}")
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            // step factories
            container.RegisterAssemblyTypes(assembly)
                .InNamespace($"{ns}.{nameof(Framework.Factories)}")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // messages
            container.RegisterAssemblyTypes(assembly)
                .InNamespace($"{ns}.{nameof(Message)}")
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return container;
        }
    }
}
