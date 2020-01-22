using System.Threading;
using GazeMonitoring.EyeTracker.Core.Calibration;
using GazeMonitoring.EyeTracker.Core.Discovery;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.IoC;
using Tobii.Interaction;
using Tobii.Interaction.Client;

namespace TobiiCoreMonitoring {
    public class TobiiDiscovery : IDiscoverable {
        public DiscoveryResult Discover(IoContainerBuilder containerBuilder) {
            var discoveryResult = new DiscoveryResult();
            var host = new Host();

            for (int i = 0; i < 5; i++) {
                if (host.Context.ConnectionState == ConnectionState.Connected)
                {
                    containerBuilder.Register<IGazePointStreamFactory, TobiiCoreGazePointStreamFactory>();
                    containerBuilder.RegisterSingleton(host);
                    containerBuilder.Register<IEyeTrackerStatusProvider, TobiiStatusProvider>();
                    containerBuilder.Register<ICalibrationManager, TobiiCalibrationManager>();
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
