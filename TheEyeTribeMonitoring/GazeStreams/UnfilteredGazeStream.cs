using EyeTribe.ClientSdk.Data;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;

namespace TheEyeTribeMonitoring.GazeStreams {
    public class UnfilteredGazeStream : GazePointStream, IFilteredGazeDataPublisher {
        public void PublishFilteredData(GazeData gazeData) {
            OnGazePointReceived(new GazePointReceivedEventArgs {GazePoint = new GazeMonitoring.Common.Entities.GazePoint {
                TimeStamp = gazeData.TimeStamp,
                X = gazeData.RawCoordinates.X,
                Y = gazeData.RawCoordinates.Y
            }});
        }
    }
}
