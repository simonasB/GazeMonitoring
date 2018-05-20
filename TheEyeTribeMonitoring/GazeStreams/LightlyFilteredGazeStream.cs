using EyeTribe.ClientSdk.Data;
using GazeMonitoring.Model;

namespace TheEyeTribeMonitoring.GazeStreams {
    public class LightlyFilteredGazeStream : EyeTribeBaseGazeStream {
        public override void PublishFilteredData(GazeData gazeData) {
            base.PublishFilteredData(gazeData);
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
