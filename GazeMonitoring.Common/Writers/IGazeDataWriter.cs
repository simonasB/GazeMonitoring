using GazeMonitoring.Model;

namespace GazeMonitoring.Common.Writers {
    public interface IGazeDataWriter {
        void Write(GazePoint gazePoint);
    }
}
