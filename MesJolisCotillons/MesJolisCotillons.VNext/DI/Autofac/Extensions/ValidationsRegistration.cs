namespace MesJolisCotillons.VNext.DI.Autofac.Extensions
{
    using global::Autofac;
    using MesJolisCotillons.Response.Builders;
    using MesJolisCotillons.VNext.Controllers.Validation;

    public static class ValidationsRegistration
    {
        public static ContainerBuilder RegisterValidationServices(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<RequestValidatorService>()
                .As<IRequestValidatorService>()
                .InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
