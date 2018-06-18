using Lamar;
using Topshelf.HostConfigurators;

namespace Topshelf.Lamar
{
	public static class HostConfiguratorExtensions
	{
		public static HostConfigurator UseLamar(this HostConfigurator configurator, IContainer container) {
			configurator.AddConfigurator(new LamarBuilderConfigurator(container));
			return configurator;
		}

		public static HostConfigurator UseLamar(this HostConfigurator configurator, Container registry) {
			configurator.AddConfigurator(new LamarBuilderConfigurator(registry));
			return configurator;
		}
	}
}