using System;
using EyeTribe.ClientSdk.Data;
using GazeMonitoring.Model;

namespace TheEyeTribeMonitoring.GazeStreams {
    public class UnfilteredGazeStream : EyeTribeBaseGazeStream {
        public override void PublishFilteredData(GazeData gazeData) {
            base.PublishFilteredData(gazeData);
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
