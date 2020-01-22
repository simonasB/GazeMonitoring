using EyeTribe.ClientSdk;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.IoC;

namespace TheEyeTribeMonitoring {
    public class EyeTribeModule : IoCModule {
        public void Load(IoContainerBuilder builder) {
            GazeManager.Instance.Activate(GazeManagerCore.ApiVersion.VERSION_1_0);
            builder.Register<IGazePointStreamFactory, EyeTribeGazePointStreamFactory>();
            builder.Register<IEyeTrackerStatusProvider, EyeTribeStatusProvider>();
        }
    }
}
