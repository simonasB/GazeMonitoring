using EyeTribe.ClientSdk.Data;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;

namespace TheEyeTribeMonitoring.GazeStreams {
    public class SensitiveFixationGazeStream : GazePointStream, IFilteredGazeDataPublisher {
        public void PublishFilteredData(GazeData gazeData) {
            if (gazeData.IsFixated) {
                OnGazePointReceived(new GazePointReceivedEventArgs {
                    GazePoint = new GazePoint {
                        Timestamp = gazeData.TimeStamp,
                        X = gazeData.RawCoordinates.X,
                        Y = gazeData.RawCoordinates.Y
                    }
                });
            }
        }
    }
}
