using System;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;
using TheEyeTribeMonitoring.GazeStreams;

namespace TheEyeTribeMonitoring {
    public class EyeTribeGazePointStreamFactory : IGazePointStreamFactory {
        public GazePointStream GetGazePointStream(DataStream dataStream) {
            switch (dataStream) {
                case DataStream.UnfilteredGaze:
                    return new UnfilteredGazeStream();
                case DataStream.LightlyFilteredGaze:
                    return new LightlyFilteredGazeStream();
                case DataStream.SensitiveFixation:
                    return new SensitiveFixationGazeStream();
                case DataStream.SlowFixation:
                    return new SlowFixationGazeStream();
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }
        }
    }
}
