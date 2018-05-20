using GazeMonitoring.Common;
using GazeMonitoring.Model;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring.GazeStreams {
    public sealed class SlowFixationGazeStream : TobiiCoreBaseGazeStream {
        public SlowFixationGazeStream(Host host, IScreenParametersProvider screenParametersProvider) : base(screenParametersProvider) {
            /*host.Streams.CreateFixationDataStream(FixationDataMode.Slow).Next += (sender, data) => {
                OnGazePointReceived(new GazePointReceivedEventArgs {
                    GazePoint = new GazePoint {
                        X = data.Data.X,
                        Y = data.Data.Y,
                        Timestamp = (long) (data.Data.Timestamp * 1000)
                    }
                });
            };*/

            var stream = host.Streams.CreateFixationDataStream(FixationDataMode.Slow);
            stream.Begin((x, y, timestamp) => {
                OnGazePointReceived(new GazePointReceivedEventArgs
                {
                    GazePoint = new GazePoint
                    {
                        X = x,
                        Y = y,
                        Timestamp = (long)(timestamp * 1000)
                    }
                });
            });

            stream.End((x, y, timestamp) => {
                OnGazePointReceived(new GazePointReceivedEventArgs
                {
                    GazePoint = new GazePoint
                    {
                        X = x,
                        Y = y,
                        Timestamp = (long)(timestamp * 1000)
                    }
                });
            });
        }
    }
}
