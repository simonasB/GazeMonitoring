using System.Threading.Tasks;

namespace GazeMonitoring.EyeTracker.Core.Status
{
    /// <summary>
    /// Default implementation of IEyeTrackerStatusProvider which necessary for app not to crash while there is no eyetrackers connected
    /// </summary>
    public class NullEyeTrackerStatusProvider : IEyeTrackerStatusProvider
    {
        /// <summary>
        /// Default implementation of IEyeTrackerStatusProvider.
        /// </summary>
        /// <returns> Returns that Eyetracker is not available.</returns>
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
