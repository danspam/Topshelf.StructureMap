using Quartz;
using Quartz.Core;
using Quartz.Impl;
using Quartz.Spi;

namespace Topshelf.Quartz.Lamar
{
	public class LamarSchedulerFactory : StdSchedulerFactory
	{
		private readonly IJobFactory _jobFactory;

		public LamarSchedulerFactory(IJobFactory jobFactory) {
			_jobFactory = jobFactory;
		}

		protected override IScheduler Instantiate(QuartzSchedulerResources resources, QuartzScheduler scheduler) {
			scheduler.JobFactory = _jobFactory;
			return base.Instantiate(resources, scheduler);
		}
	}
}