using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring.GazeStreams {
    public sealed class LightlyFilteredGazeStream : GazePointStream {
        public LightlyFilteredGazeStream(Host host) {
            host.Streams.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered).Next += (sender, data) => {
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
