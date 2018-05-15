using System.Threading.Tasks;

namespace GazeMonitoring.EyeTracker.Core.Status {
    public interface IEyeTrackerStatusProvider {
        Task<EyeTrackerStatus> GetStatusAsync();
    }
}
