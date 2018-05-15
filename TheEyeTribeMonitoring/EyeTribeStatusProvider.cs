using System.Threading.Tasks;
using EyeTribe.ClientSdk;
using GazeMonitoring.EyeTracker.Core;
using GazeMonitoring.EyeTracker.Core.Status;

namespace TheEyeTribeMonitoring {
    public class EyeTribeStatusProvider : IEyeTrackerStatusProvider {
        public async Task<EyeTrackerStatus> GetStatusAsync() {
            if (!GazeManager.Instance.IsActivated) {
                await GazeManager.Instance.ActivateAsync();
            }

            var status = new EyeTrackerStatus {
                IsAvailable = GazeManager.Instance.Trackerstate == GazeManagerCore.TrackerState.TRACKER_CONNECTED,
                Name = "EyeTribe"
            };

            return status;
        }
    }
}
