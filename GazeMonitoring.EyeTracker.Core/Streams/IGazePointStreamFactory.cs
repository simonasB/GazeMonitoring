using GazeMonitoring.Model;

namespace GazeMonitoring.EyeTracker.Core.Streams {
    /// <summary>
    /// Provides abstraction for creating eyetracker specific gaze point stream
    /// </summary>
    public interface IGazePointStreamFactory {
        /// <summary>
        /// Creates gaze point stream based on specified datastream
        /// </summary>
        /// <param name="dataStream">Based on this parameter different type of stream is created. Should be implemented for each type of data stream.</param>
        /// <returns>Eyetracker specific gaze point stream</returns>
        GazePointStream GetGazePointStream(DataStream dataStream);
    }
}
