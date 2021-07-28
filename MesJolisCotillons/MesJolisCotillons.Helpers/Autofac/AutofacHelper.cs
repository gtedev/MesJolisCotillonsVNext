using Autofac;
using System;

namespace MesJolisCotillons.Helpers.Autofac
{
    public static class AutofacHelper
    {
        public static ContainerBuilder RegisterTypeGeneric(this ContainerBuilder container, Type type)
        {
            container
                .RegisterGeneric(type)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return container;
        }
    }
}
