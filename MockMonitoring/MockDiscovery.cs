using Autofac;
using GazeMonitoring.EyeTracker.Core.Discovery;
using GazeMonitoring.EyeTracker.Core.Streams;

namespace MockMonitoring {
    public class MockDiscovery : IDiscoverable {
        public DiscoveryResult Discover(ContainerBuilder container) {
            container.RegisterType<MockGazePointStreamFactory>().As<IGazePointStreamFactory>();
            return new DiscoveryResult {
                IsActive = true
            };
        }
    }
}
