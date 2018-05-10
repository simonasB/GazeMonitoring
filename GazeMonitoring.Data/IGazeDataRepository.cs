using GazeMonitoring.Model;

namespace GazeMonitoring.Data {
    public interface IGazeDataRepository {
        void SaveGazePoint(GazePoint gazePoint);
        void SaveSaccade(Saccade saccade);
    }
}
