using GazeMonitoring.Common;
using GazeMonitoring.Model;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring.GazeStreams {
    public sealed class LightlyFilteredGazeStream : TobiiCoreBaseGazeStream {
        public LightlyFilteredGazeStream(Host host, IScreenParameters screenParameters) : base(screenParameters) {
            host.Streams.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered).Next += (sender, data) => {
                OnGazePointReceived(new GazePointReceivedEventArgs {
                    GazePoint = new GazePoint {
                        X = data.Data.X,
                        Y = data.Data.Y,
                        Timestamp = (long)(data.Data.Timestamp)
                    }
                });
            };
        }
    }
}
