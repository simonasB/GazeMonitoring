using EyeTribe.ClientSdk.Data;
using GazeMonitoring.EyeTracker.Core.Streams;

namespace TheEyeTribeMonitoring.GazeStreams {
    public abstract class EyeTribeBaseGazeStream : GazePointStream, IFilteredGazeDataPublisher {
        public abstract void PublishFilteredData(GazeData gazeData);
    }
}