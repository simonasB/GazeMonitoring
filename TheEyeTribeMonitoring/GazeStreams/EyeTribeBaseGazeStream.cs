using System;
using EyeTribe.ClientSdk.Data;
using GazeMonitoring.EyeTracker.Core.Streams;

namespace TheEyeTribeMonitoring.GazeStreams {
    public abstract class EyeTribeBaseGazeStream : GazePointStream, IFilteredGazeDataPublisher {
        public virtual void PublishFilteredData(GazeData gazeData) {
            gazeData.TimeStamp = (long)(DateTime.Parse(gazeData.TimeStampString).ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}