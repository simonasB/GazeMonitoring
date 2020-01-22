using EyeTribe.ClientSdk;
using GazeMonitoring.EyeTracker.Core.Discovery;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.IoC;

namespace TheEyeTribeMonitoring {
    public class EyeTribeDiscovery : IDiscoverable {
        public DiscoveryResult Discover(IoContainerBuilder container)
        {
            var discoveryResult = new DiscoveryResult();
            GazeManager.Instance.Activate(GazeManagerCore.ApiVersion.VERSION_1_0);

            if (GazeManager.Instance.Trackerstate == GazeManagerCore.TrackerState.TRACKER_CONNECTED)
            {
                discoveryResult.IsActive = true;
                container.Register<IGazePointStreamFactory, EyeTribeGazePointStreamFactory>();
                container.Register<IEyeTrackerStatusProvider, EyeTribeStatusProvider>();
            }
            else
            {
                discoveryResult.IsActive = false;
                GazeManager.Instance.Deactivate();
            }

            return discoveryResult;
        }
    }
}
