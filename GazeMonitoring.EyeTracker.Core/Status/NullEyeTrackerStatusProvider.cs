using System.Threading.Tasks;

namespace GazeMonitoring.EyeTracker.Core.Status
{
    public class NullEyeTrackerStatusProvider : IEyeTrackerStatusProvider
    {
        public Task<EyeTrackerStatus> GetStatusAsync()
        {
            return Task.FromResult(new EyeTrackerStatus
            {
                IsAvailable = false,
                Name = "Unavailable"
            });
        }
    }
}
