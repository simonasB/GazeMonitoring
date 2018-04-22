using GazeMonitoring.Model;

namespace GazeMonitoring.EyeTracker.Core.Streams {
    public interface IGazePointStreamFactory {
        GazePointStream GetGazePointStream(DataStream dataStream);
    }
}
