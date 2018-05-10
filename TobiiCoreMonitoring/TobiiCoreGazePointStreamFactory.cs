using System;
using GazeMonitoring.Common;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;
using Tobii.Interaction;
using TobiiCoreMonitoring.GazeStreams;

namespace TobiiCoreMonitoring {
    public class TobiiCoreGazePointStreamFactory : IGazePointStreamFactory {
        private readonly Host _host;
        private readonly IScreenParametersProvider _screenParametersProvider;

        public TobiiCoreGazePointStreamFactory(Host host, IScreenParametersProvider screenParametersProvider) {
            if (host == null) {
                throw new ArgumentNullException(nameof(host));
            }
            if (screenParametersProvider == null) {
                throw new ArgumentNullException(nameof(screenParametersProvider));
            }

            _host = host;
            _screenParametersProvider = screenParametersProvider;
        }

        public GazePointStream GetGazePointStream(DataStream dataStream) {
            switch (dataStream)
            {
                case DataStream.UnfilteredGaze:
                    return new UnfilteredGazeStream(_host, _screenParametersProvider);
                case DataStream.LightlyFilteredGaze:
                    return new LightlyFilteredGazeStream(_host, _screenParametersProvider);
                case DataStream.SensitiveFixation:
                    return new SensitiveFixationGazeStream(_host, _screenParametersProvider);
                case DataStream.SlowFixation:
                    return new SlowFixationGazeStream(_host, _screenParametersProvider);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }
        }
    }
}
