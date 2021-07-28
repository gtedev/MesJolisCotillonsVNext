namespace MesJolisCotillons.VNext.DI.Autofac.Extensions
{
    using global::Autofac;
    using MesJolisCotillons.Core.Framework.Factories;
    using MesJolisCotillons.Operations.Service;
    using MesJolisCotillons.VNext.DI.Autofac.Factories;

    public static class OperationsRegistration
    {
        public static ContainerBuilder RegisterOperationsServices(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<OperationRunnerService>()
                .As<IOperationRunnerService>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<OperationFactory>()
                .As<IOperationFactory>()
                .InstancePerLifetimeScope();

            ////containerBuilder.RegisterGeneric(typeof(OperationBuilder<,>))
            ////    .AsImplementedInterfaces()
            ////    .InstancePerLifetimeScope();

            ////var bootstrapType = typeof(Bootstrap);
            ////containerBuilder.RegisterAssemblyTypes(bootstrapType.Assembly)
            ////    .InNamespace($"{bootstrapType.Namespace}.Commands")
            ////    .AsSelf()
            ////    .InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
