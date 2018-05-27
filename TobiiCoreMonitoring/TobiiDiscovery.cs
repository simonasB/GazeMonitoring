using System.Threading;
using Autofac;
using GazeMonitoring.EyeTracker.Core.Discovery;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.EyeTracker.Core.Streams;
using Tobii.Interaction;
using Tobii.Interaction.Client;

namespace TobiiCoreMonitoring {
    public class TobiiDiscovery : IDiscoverable {
        public DiscoveryResult Discover(ContainerBuilder containerBuilder) {
            var discoveryResult = new DiscoveryResult();
            var host = new Host();

            for (int i = 0; i < 5; i++) {
                if (host.Context.ConnectionState == ConnectionState.Connected) {
                    containerBuilder.RegisterType<TobiiCoreGazePointStreamFactory>().As<IGazePointStreamFactory>();
                    containerBuilder.RegisterInstance(host).SingleInstance();
                    containerBuilder.RegisterType<TobiiStatusProvider>().As<IEyeTrackerStatusProvider>();
                    discoveryResult.IsActive = true;
                    return discoveryResult;
                }

                Thread.Sleep(50);
            }

            host.Dispose();
            discoveryResult.IsActive = false;

            return discoveryResult;
        }
    }
}
