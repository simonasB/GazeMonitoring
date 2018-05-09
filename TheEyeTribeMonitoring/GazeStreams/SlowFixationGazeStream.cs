using EyeTribe.ClientSdk.Data;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;

namespace TheEyeTribeMonitoring.GazeStreams {
    public class SlowFixationGazeStream : GazePointStream, IFilteredGazeDataPublisher {
        public void PublishFilteredData(GazeData gazeData) {
            if (gazeData.IsFixated) {
                OnGazePointReceived(new GazePointReceivedEventArgs {
                    GazePoint = new GazePoint {
                        Timestamp = gazeData.TimeStamp,
                        X = gazeData.SmoothedCoordinates.X,
                        Y = gazeData.SmoothedCoordinates.Y
                    }
                });
            }
        }
    }
}
