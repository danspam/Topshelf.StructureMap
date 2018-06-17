﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using Topshelf.HostConfigurators;
using Topshelf.ServiceConfigurators;
using Topshelf.StructureMap;
using SimpleStructureMapJobFactory = Topshelf.Quartz.StructureMap.SimpleJobFactory;

namespace Topshelf.Quartz.StructureMap
{
    public static class StructureMapScheduleConfiguratorExtensions
    {
        public static ServiceConfigurator<T> UseQuartzStructureMap<T>(this ServiceConfigurator<T> configurator, bool withNestedContainers = true)
              where T : class
        {
            SetupStructureMap(withNestedContainers);

            return configurator;
        }

        public static ServiceConfigurator UseQuartzStructureMap(this ServiceConfigurator configurator, bool withNestedContainers = true)
        {
            SetupStructureMap(withNestedContainers);
            return configurator;
        }

        public static HostConfigurator UseQuartzStructureMap(this HostConfigurator configurator, bool withNestedContainers = false)
        {
            SetupStructureMap(withNestedContainers);
            return configurator;
        }

        internal static void SetupStructureMap(bool withNestedContainers)
        {
            var container = StructureMapBuilderConfigurator.Container;
            if (container == null)
                throw new Exception("You must call UseStructureMap() to use the Quartz Topshelf Structuremap integration.");

            container.Configure(c =>
            {
                if (withNestedContainers)
                    c.AddScoped<IJobFactory, NestedContainerJobFactory>();
                else
                    c.AddTransient<IJobFactory, SimpleStructureMapJobFactory>();

                c.AddTransient<ISchedulerFactory, StructureMapSchedulerFactory>();
                c.AddSingleton(typeof(Task<IScheduler>),
                    ctx => ctx.GetService<ISchedulerFactory>().GetScheduler(CancellationToken.None));
            });

            ScheduleJobServiceConfiguratorExtensions.SchedulerFactory = () => container.GetInstance<Task<IScheduler>>();
        }
    }
}
