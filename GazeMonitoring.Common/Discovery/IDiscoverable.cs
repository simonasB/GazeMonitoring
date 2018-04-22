using Autofac;

namespace GazeMonitoring.Common.Discovery {
    public interface IDiscoverable {
        DiscoveryResult Discover(ContainerBuilder container);
    }
}
