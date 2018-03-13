using Autofac;
using GazeMonitoring.Common;
using Tobii.Interaction;
using Tobii.Interaction.Client;

namespace TobiiCoreMonitoring {
    public class TobiiDiscovery : IDiscoverable {
        public DiscoveryResult Discover(ContainerBuilder containerBuilder) {
            var discoveryResult = new DiscoveryResult();
            var host = new Host();
            if (host.Context.ConnectionState == ConnectionState.Connected) {
                containerBuilder.RegisterType<TobiiCoreGazePointStreamFactory>().As<IGazePointStreamFactory>();
                containerBuilder.RegisterInstance(host);
                discoveryResult.IsActive = true;
            }
            else {
                host.Dispose();
                discoveryResult.IsActive = false;
            }

            return discoveryResult;
        }
    }
}
