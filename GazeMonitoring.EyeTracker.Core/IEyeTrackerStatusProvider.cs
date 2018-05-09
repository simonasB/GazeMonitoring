using System.Threading.Tasks;

namespace GazeMonitoring.EyeTracker.Core {
    public interface IEyeTrackerStatusProvider {
        Task<EyeTrackerStatus> GetStatusAsync();
    }
}
