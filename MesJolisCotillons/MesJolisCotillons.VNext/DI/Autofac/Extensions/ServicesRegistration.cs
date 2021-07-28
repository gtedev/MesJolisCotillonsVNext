namespace MesJolisCotillons.VNext.DI.Autofac.Extensions
{
    using global::Autofac;
    using MesJolisCotillons.VNext.Controllers.Service;

    public static class ServicesRegistration
    {
        public static ContainerBuilder RegisterServices(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ControllerService>()
                .As<IControllerService>()
                .InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
