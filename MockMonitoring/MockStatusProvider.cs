using System.Threading.Tasks;
using GazeMonitoring.EyeTracker.Core.Status;

namespace MockMonitoring {
    public class MockStatusProvider : IEyeTrackerStatusProvider {
        public Task<EyeTrackerStatus> GetStatusAsync() {
            return Task.FromResult(new EyeTrackerStatus {
                IsAvailable = false,
                Name = "Mock"
            });
        }
    }
}
