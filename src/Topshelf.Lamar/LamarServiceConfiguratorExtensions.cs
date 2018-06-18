using System;
using Topshelf.ServiceConfigurators;

namespace Topshelf.Lamar
{
    public static class LamarServiceConfiguratorExtensions
    {
        public static Func<T> GetFactory<T>() where T : class => () => LamarBuilderConfigurator.Container.GetInstance<T>();

        public static ServiceConfigurator<T> ConstructUsingLamar<T>(this ServiceConfigurator<T> configurator) where T : class {

            if (typeof(T).GetInterfaces()?.Length > 0 && typeof(ServiceControl).IsAssignableFrom(typeof(T))) {
                configurator.WhenStarted((service, control) => ((ServiceControl)service).Start(control));
                configurator.WhenStopped((service, control) => ((ServiceControl)service).Stop(control));
            }

            configurator.ConstructUsing(GetFactory<T>());
            return configurator;
        }
    }
}