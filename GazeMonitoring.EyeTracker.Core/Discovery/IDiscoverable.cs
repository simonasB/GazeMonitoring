using Autofac;

namespace GazeMonitoring.EyeTracker.Core.Discovery {
    public interface IDiscoverable {
        DiscoveryResult Discover(ContainerBuilder container);
    }
}
