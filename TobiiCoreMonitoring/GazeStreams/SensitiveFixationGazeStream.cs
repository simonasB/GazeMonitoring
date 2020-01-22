using GazeMonitoring.Model;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring.GazeStreams {
    public sealed class SensitiveFixationGazeStream : TobiiCoreBaseGazeStream {
        private double _lastFixationStartTime;
        private FixationPoint _lastFixationPoint;

        public SensitiveFixationGazeStream(Host host, IScreenParameters screenParameters) : base(screenParameters) {
            var stream = host.Streams.CreateFixationDataStream(FixationDataMode.Sensitive);
            stream.Begin((x, y, timestamp) => {
                _lastFixationPoint = new FixationPoint
                {
                    X = x,
                    Y = y,
                    Timestamp = (long)(timestamp)
                };
                _lastFixationStartTime = timestamp;
            });

            stream.End((x, y, timestamp) => {
                if (_lastFixationPoint != null)
                {
                    _lastFixationPoint.DurationInMillis = (long) (timestamp - _lastFixationStartTime);
                    OnGazePointReceived(new GazePointReceivedEventArgs
                    {
                        GazePoint = _lastFixationPoint
                    });
                }
            });
        }
    }
}
