using Autofac;
using EyeTribe.ClientSdk;
using GazeMonitoring.Common;

namespace TheEyeTribeMonitoring {
    public class EyeTribeDiscovery : IDiscoverable {
        public DiscoveryResult Discover(ContainerBuilder container) {
            var discoveryResult = new DiscoveryResult();
            GazeManager.Instance.Activate(GazeManagerCore.ApiVersion.VERSION_1_0);

            if (GazeManager.Instance.Trackerstate == GazeManagerCore.TrackerState.TRACKER_CONNECTED) {
                discoveryResult.IsActive = true;
                container.RegisterType<EyeTribeGazePointStreamFactory>().As<IGazePointStreamFactory>();
            } else {
                discoveryResult.IsActive = false;
                GazeManager.Instance.Deactivate();
            }

            return discoveryResult;
        }
    }
}
