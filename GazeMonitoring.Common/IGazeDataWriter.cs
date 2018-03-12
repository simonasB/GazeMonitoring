using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Common {
    public interface IGazeDataWriter {
        void Write(GazePoint gazePoint);
    }
}
