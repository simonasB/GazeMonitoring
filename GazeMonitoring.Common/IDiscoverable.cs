using Autofac;

namespace GazeMonitoring.Common {
    public interface IDiscoverable {
        DiscoveryResult Discover(ContainerBuilder container);
    }
}
