using System;
using System.Collections.Generic;
using Lamar;
using Topshelf.Builders;
using Topshelf.Configurators;
using Topshelf.HostConfigurators;

namespace Topshelf.StructureMap
{
	public class StructureMapBuilderConfigurator : HostBuilderConfigurator
	{
		private static IContainer _container;

		public static IContainer Container => _container;

		public StructureMapBuilderConfigurator(ServiceRegistry registry) {
			if (registry == null)
				throw new ArgumentNullException(nameof(registry));
			_container = new Container(registry);
		}

		public StructureMapBuilderConfigurator(IContainer container)
		{
			_container = container ?? throw new ArgumentNullException(nameof(container));
		}

		public IEnumerable<ValidateResult> Validate() {
			yield break;
		}

		public HostBuilder Configure(HostBuilder builder) => builder;
	}
}