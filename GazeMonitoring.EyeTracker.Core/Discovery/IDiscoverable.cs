using Autofac;
using GazeMonitoring.IoC;

namespace GazeMonitoring.EyeTracker.Core.Discovery {
    public interface IDiscoverable {
        DiscoveryResult Discover(IoContainerBuilder container);
    }
}
