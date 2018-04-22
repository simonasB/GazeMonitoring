using GazeMonitoring.Common.Streams;
using GazeMonitoring.Model;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring.GazeStreams {
    public sealed class UnfilteredGazeStream : GazePointStream {
        public UnfilteredGazeStream(Host host) {
            host.Streams.CreateGazePointDataStream(GazePointDataMode.Unfiltered).Next += (sender, data) => {
                OnGazePointReceived(new GazePointReceivedEventArgs {
                    GazePoint = new GazePoint {
                        X = data.Data.X,
                        Y = data.Data.Y,
                        Timestamp = data.Data.Timestamp
                    }
                });
            };
        }
    }
}
