using System;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;

namespace TobiiCoreMonitoring.GazeStreams {
    public abstract class TobiiCoreBaseGazeStream : GazePointStream {
        private readonly IScreenParameters _screenParameters;
        private const int DefaultTimestampValue = -1;

        private long _firstReceivedUnixTimestamp = DefaultTimestampValue;
        private long _firstReceivedTrackerTimestamp = DefaultTimestampValue;

        protected TobiiCoreBaseGazeStream(IScreenParameters screenParameters) {
            _screenParameters = screenParameters;
        }

        protected override void OnGazePointReceived(GazePointReceivedEventArgs e) {
            if (_firstReceivedUnixTimestamp == -1) {
                _firstReceivedUnixTimestamp = (long) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                _firstReceivedTrackerTimestamp = e.GazePoint.Timestamp;
            }
            e.GazePoint.Timestamp = _firstReceivedUnixTimestamp + e.GazePoint.Timestamp - _firstReceivedTrackerTimestamp;
            //e.GazePoint.Y = _screenParameters.Height - e.GazePoint.Y;
            base.OnGazePointReceived(e);
        }
    }
}
