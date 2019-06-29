using System.Threading.Tasks;

namespace GazeMonitoring.EyeTracker.Core.Status {
    /// <summary>
    /// Responsible for providing eyetracker status.
    /// </summary>
    public interface IEyeTrackerStatusProvider {
        /// <summary>
        /// Gets eyetracker status. This method might be called each 10 seconds so should not be blocking longer than that.
        /// </summary>
        /// <returns>Eyetrackerstatus object</returns>
        Task<EyeTrackerStatus> GetStatusAsync();
    }
}
