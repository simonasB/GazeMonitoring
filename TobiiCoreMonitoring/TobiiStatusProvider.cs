using System.Threading.Tasks;
using GazeMonitoring.EyeTracker.Core.Status;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring {
    public class TobiiStatusProvider : IEyeTrackerStatusProvider {
        private readonly Host _host;

        public TobiiStatusProvider(Host host) {
            _host = host;
        }

        public async Task<EyeTrackerStatus> GetStatusAsync() {
            var stateBag = await _host.Context.GetStateAsync("eyeTracking.state");
            stateBag.TryGetStateValue<EyeTrackingDeviceStatus>("eyeTracking.state", out var stateValue);

            return new EyeTrackerStatus {
                IsAvailable = stateValue == EyeTrackingDeviceStatus.Tracking,
                Name = "Tobii 4C"
            };
        }
    }
}
