using System;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;
using Tobii.Interaction;
using TobiiCoreMonitoring.GazeStreams;

namespace TobiiCoreMonitoring {
    public class TobiiCoreGazePointStreamFactory : IGazePointStreamFactory {
        private readonly Host _host;

        public TobiiCoreGazePointStreamFactory(Host host) {
            if (host == null) {
                throw new ArgumentNullException(nameof(host));
            }

            _host = host;
        }

        public GazePointStream GetGazePointStream(DataStream dataStream) {
            switch (dataStream)
            {
                case DataStream.UnfilteredGaze:
                    return new UnfilteredGazeStream(_host);
                case DataStream.LightlyFilteredGaze:
                    return new LightlyFilteredGazeStream(_host);
                case DataStream.SensitiveFixation:
                    return new SensitiveFixationGazeStream(_host);
                case DataStream.SlowFixation:
                    return new SlowFixationGazeStream(_host);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }
        }
    }
}
