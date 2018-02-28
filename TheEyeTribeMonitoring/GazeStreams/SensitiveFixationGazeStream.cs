using EyeTribe.ClientSdk.Data;
using GazeMonitoring.Common;

namespace TheEyeTribeMonitoring.GazeStreams {
    public class SensitiveFixationGazeStream : GazePointStream, IFilteredGazeDataPublisher {
        public void PublishFilteredData(GazeData gazeData) {
            throw new System.NotImplementedException();
        }
    }
}
