using System;
using GazeMonitoring.Common;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;

namespace TobiiCoreMonitoring.GazeStreams {
    public abstract class TobiiCoreBaseGazeStream : GazePointStream {
        private readonly IScreenParametersProvider _screenParametersProvider;
        private const int DefaultTimestampValue = -1;

        private long _firstReceivedUnixTimestamp = DefaultTimestampValue;
        private long _firstReceivedTrackerTimestamp = DefaultTimestampValue;

        protected TobiiCoreBaseGazeStream(IScreenParametersProvider screenParametersProvider) {
            _screenParametersProvider = screenParametersProvider;
        }

        protected override void OnGazePointReceived(GazePointReceivedEventArgs e) {
            if (_firstReceivedUnixTimestamp == -1) {
                _firstReceivedUnixTimestamp = (long) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                _firstReceivedTrackerTimestamp = e.GazePoint.Timestamp;
            }
            e.GazePoint.Timestamp = _firstReceivedUnixTimestamp + e.GazePoint.Timestamp - _firstReceivedTrackerTimestamp;
            //e.GazePoint.Y = _screenParametersProvider.Height - e.GazePoint.Y;
            base.OnGazePointReceived(e);
        }
    }
}
