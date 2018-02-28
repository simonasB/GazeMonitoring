using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Common {
    public interface IGazePointStreamFactory {
        GazePointStream GetGazePointStream(DataStream dataStream);
    }
}
