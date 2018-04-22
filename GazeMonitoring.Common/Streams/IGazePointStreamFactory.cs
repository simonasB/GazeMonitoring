using GazeMonitoring.Model;

namespace GazeMonitoring.Common.Streams {
    public interface IGazePointStreamFactory {
        GazePointStream GetGazePointStream(DataStream dataStream);
    }
}
