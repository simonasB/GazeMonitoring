using System;
using EyeTribe.ClientSdk;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;
using TheEyeTribeMonitoring.GazeStreams;

namespace TheEyeTribeMonitoring {
    public class EyeTribeGazePointStreamFactory : IGazePointStreamFactory {
        public GazePointStream GetGazePointStream(DataStream dataStream) {
            IGazeListener gazeListener;

            switch (dataStream) {
                case DataStream.UnfilteredGaze:
                    var unfilteredGazeStream = new UnfilteredGazeStream();
                    gazeListener = new GazeListener(unfilteredGazeStream);
                    GazeManager.Instance.AddGazeListener(gazeListener);
                    return unfilteredGazeStream;
                case DataStream.LightlyFilteredGaze:
                    var lightlyFilteredGazeStream = new LightlyFilteredGazeStream();
                    gazeListener = new GazeListener(lightlyFilteredGazeStream);
                    GazeManager.Instance.AddGazeListener(gazeListener);
                    return lightlyFilteredGazeStream;
                case DataStream.SensitiveFixation:
                    var sensitiveFixationGazeStream = new SensitiveFixationGazeStream();
                    gazeListener = new GazeListener(sensitiveFixationGazeStream);
                    GazeManager.Instance.AddGazeListener(gazeListener);
                    return sensitiveFixationGazeStream;
                case DataStream.SlowFixation:
                    var slowFixationGazeStream = new SlowFixationGazeStream();
                    gazeListener = new GazeListener(slowFixationGazeStream);
                    GazeManager.Instance.AddGazeListener(gazeListener);
                    return slowFixationGazeStream;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }
        }
    }
}
