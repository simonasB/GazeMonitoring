using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Writers {
    public interface IGazeDataWriter {
        void Write(GazePoint gazePoint);
    }
}
