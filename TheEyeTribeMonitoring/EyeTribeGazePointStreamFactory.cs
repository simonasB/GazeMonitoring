using System;
using EyeTribe.ClientSdk;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;
using TheEyeTribeMonitoring.GazeStreams;

namespace TheEyeTribeMonitoring {
    public class EyeTribeGazePointStreamFactory : IGazePointStreamFactory, IDisposable {
        private IGazeListener _gazeListener;

        public GazePointStream GetGazePointStream(DataStream dataStream) {
            switch (dataStream) {
                case DataStream.UnfilteredGaze:
                    var unfilteredGazeStream = new UnfilteredGazeStream();
                    _gazeListener = new GazeListener(unfilteredGazeStream);
                    GazeManager.Instance.AddGazeListener(_gazeListener);
                    return unfilteredGazeStream;
                case DataStream.LightlyFilteredGaze:
                    var lightlyFilteredGazeStream = new LightlyFilteredGazeStream();
                    _gazeListener = new GazeListener(lightlyFilteredGazeStream);
                    GazeManager.Instance.AddGazeListener(_gazeListener);
                    return lightlyFilteredGazeStream;
                case DataStream.SensitiveFixation:
                    var sensitiveFixationGazeStream = new SensitiveFixationGazeStream();
                    _gazeListener = new GazeListener(sensitiveFixationGazeStream);
                    GazeManager.Instance.AddGazeListener(_gazeListener);
                    return sensitiveFixationGazeStream;
                case DataStream.SlowFixation:
                    var slowFixationGazeStream = new SlowFixationGazeStream();
                    _gazeListener = new GazeListener(slowFixationGazeStream);
                    GazeManager.Instance.AddGazeListener(_gazeListener);
                    return slowFixationGazeStream;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }
        }

        public void Dispose() {
            GazeManager.Instance.RemoveGazeListener(_gazeListener);
        }
    }
}
