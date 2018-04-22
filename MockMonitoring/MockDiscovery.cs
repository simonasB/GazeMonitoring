using Autofac;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Discovery;
using GazeMonitoring.Common.Streams;

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
