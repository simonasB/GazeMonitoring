using EyeTribe.ClientSdk.Data;
using GazeMonitoring.Model;

namespace TheEyeTribeMonitoring.GazeStreams {
    public class SlowFixationGazeStream : EyeTribeBaseGazeStream {
        public override void PublishFilteredData(GazeData gazeData) {
            base.PublishFilteredData(gazeData);
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
