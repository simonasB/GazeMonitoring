using EyeTribe.ClientSdk;
using GazeMonitoring.Common;

namespace TheEyeTribeMonitoring {
    public class EyeTribeInitializer : IEyeTrackerInitializer {
        private readonly IGazeListener _gazeListener;

        public EyeTribeInitializer(IGazeListener gazeListener) {
            _gazeListener = gazeListener;
        }

        public void Initialize() {
            GazeManager.Instance.Activate(GazeManagerCore.ApiVersion.VERSION_1_0);
            GazeManager.Instance.AddGazeListener(_gazeListener);
        }

        public void Dispose() {
            GazeManager.Instance.Deactivate();
        }
    }
}
