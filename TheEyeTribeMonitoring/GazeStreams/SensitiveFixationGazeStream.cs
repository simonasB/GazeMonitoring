using EyeTribe.ClientSdk.Data;
using GazeMonitoring.Model;

namespace TheEyeTribeMonitoring.GazeStreams {
    public class SensitiveFixationGazeStream : EyeTribeBaseGazeStream {
        public override void PublishFilteredData(GazeData gazeData) {
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
