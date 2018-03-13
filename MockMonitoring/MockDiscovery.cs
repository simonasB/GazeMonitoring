using Autofac;
using GazeMonitoring.Common;

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
