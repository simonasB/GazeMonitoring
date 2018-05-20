using System.Runtime.CompilerServices;
using GazeMonitoring.Common;
using GazeMonitoring.Model;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring.GazeStreams {
    public sealed class SensitiveFixationGazeStream : TobiiCoreBaseGazeStream {
        public SensitiveFixationGazeStream(Host host, IScreenParametersProvider screenParametersProvider) : base(screenParametersProvider) {
            //host.Streams.CreateFixationDataStream(FixationDataMode.Sensitive)
            /*host.Streams.CreateFixationDataStream(FixationDataMode.Sensitive).Next += (sender, data) => {
                OnGazePointReceived(new GazePointReceivedEventArgs {
                    GazePoint = new GazePoint {
                        X = data.Data.X,
                        Y = data.Data.Y,
                        Timestamp = (long) (data.Data.Timestamp * 1000)
                    }
                });
            };*/

            var stream = host.Streams.CreateFixationDataStream(FixationDataMode.Sensitive);
            stream.Begin((x, y, timestamp) => {
                OnGazePointReceived(new GazePointReceivedEventArgs {
                    GazePoint = new GazePoint {
                        X = x,
                        Y = y,
                        Timestamp = (long) (timestamp * 1000)
                    }
                });
            });

            stream.End((x, y, timestamp) => {
                OnGazePointReceived(new GazePointReceivedEventArgs {
                    GazePoint = new GazePoint {
                        X = x,
                        Y = y,
                        Timestamp = (long) (timestamp * 1000)
                    }
                });
            });
        }
    }
}
