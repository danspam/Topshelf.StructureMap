using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using Topshelf.HostConfigurators;
using Topshelf.Lamar;
using Topshelf.ServiceConfigurators;
using SimpleLamarJobFactory = Topshelf.Quartz.Lamar.SimpleJobFactory;

namespace Topshelf.Quartz.Lamar
{
    public static class LamarScheduleConfiguratorExtensions
    {
        public static ServiceConfigurator<T> UseQuartzLamar<T>(this ServiceConfigurator<T> configurator, bool withNestedContainers = true)
              where T : class
        {
            SetupLamar(withNestedContainers);

            return configurator;
        }

        public static ServiceConfigurator UseQuartzLamar(this ServiceConfigurator configurator, bool withNestedContainers = true)
        {
            SetupLamar(withNestedContainers);
            return configurator;
        }

        public static HostConfigurator UseQuartzLamar(this HostConfigurator configurator, bool withNestedContainers = false)
        {
            SetupLamar(withNestedContainers);
            return configurator;
        }

        internal static void SetupLamar(bool withNestedContainers)
        {
            var container = LamarBuilderConfigurator.Container;
            if (container == null)
                throw new Exception("You must call UseLamar() to use the Quartz Topshelf Lamar integration.");

            container.Configure(c =>
            {
                if (withNestedContainers)
                    c.AddScoped<IJobFactory, NestedContainerJobFactory>();
                else
                    c.AddTransient<IJobFactory, SimpleLamarJobFactory>();

                c.AddTransient<ISchedulerFactory, LamarSchedulerFactory>();
                c.AddSingleton(typeof(Task<IScheduler>),
                    ctx => ctx.GetService<ISchedulerFactory>().GetScheduler(CancellationToken.None));
            });

            ScheduleJobServiceConfiguratorExtensions.SchedulerFactory = () => container.GetInstance<Task<IScheduler>>();
        }
    }
}
