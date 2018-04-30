using System;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;

namespace TobiiCoreMonitoring {
    public class TobiiCoreBaseGazeStream : GazePointStream {
        private const int DefaultTimestampValue = -1;

        protected static long _firstReceivedUnixTimestamp = DefaultTimestampValue;
        protected static long _firstReceivedTrackerTimestamp = DefaultTimestampValue;

        protected override void OnGazePointReceived(GazePointReceivedEventArgs e) {
            if (_firstReceivedUnixTimestamp == -1) {
                _firstReceivedUnixTimestamp = (long) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                _firstReceivedTrackerTimestamp = e.GazePoint.Timestamp;
            }
            e.GazePoint.Timestamp = _firstReceivedUnixTimestamp + e.GazePoint.Timestamp - _firstReceivedTrackerTimestamp;
            base.OnGazePointReceived(e);
        }
    }
}
