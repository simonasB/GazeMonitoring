using System;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;
using Tobii.Interaction;
using TobiiCoreMonitoring.GazeStreams;

namespace TobiiCoreMonitoring {
    public class TobiiCoreGazePointStreamFactory : IGazePointStreamFactory {
        private readonly Host _host;
        private readonly IScreenParameters _screenParameters;

        public TobiiCoreGazePointStreamFactory(Host host, IScreenParameters screenParameters) {
            if (host == null) {
                throw new ArgumentNullException(nameof(host));
            }
            if (screenParameters == null) {
                throw new ArgumentNullException(nameof(screenParameters));
            }

            _host = host;
            _screenParameters = screenParameters;
        }

        public GazePointStream GetGazePointStream(DataStream dataStream) {
            switch (dataStream)
            {
                case DataStream.UnfilteredGaze:
                    return new UnfilteredGazeStream(_host, _screenParameters);
                case DataStream.LightlyFilteredGaze:
                    return new LightlyFilteredGazeStream(_host, _screenParameters);
                case DataStream.SensitiveFixation:
                    return new SensitiveFixationGazeStream(_host, _screenParameters);
                case DataStream.SlowFixation:
                    return new SlowFixationGazeStream(_host, _screenParameters);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }
        }
    }
}
