using GazeMonitoring.Common;
using GazeMonitoring.Model;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring.GazeStreams {
    public sealed class UnfilteredGazeStream : TobiiCoreBaseGazeStream {
        public UnfilteredGazeStream(Host host, IScreenParametersProvider screenParametersProvider) : base(screenParametersProvider) {
            host.Streams.CreateGazePointDataStream(GazePointDataMode.Unfiltered).Next += (sender, data) => {

                OnGazePointReceived(new GazePointReceivedEventArgs {
                    GazePoint = new GazePoint {
                        X = data.Data.X,
                        Y = data.Data.Y,
                        Timestamp = (long) data.Data.Timestamp
                    }
                });
            };
        }
    }
}
