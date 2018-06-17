using Lamar;
using Topshelf.HostConfigurators;

namespace Topshelf.StructureMap
{
	public static class HostConfiguratorExtensions
	{
		public static HostConfigurator UseStructureMap(this HostConfigurator configurator, IContainer container) {
			configurator.AddConfigurator(new StructureMapBuilderConfigurator(container));
			return configurator;
		}

		public static HostConfigurator UseStructureMap(this HostConfigurator configurator, Container registry) {
			configurator.AddConfigurator(new StructureMapBuilderConfigurator(registry));
			return configurator;
		}
	}
}