using System;
using System.Threading;
using System.Threading.Tasks;
using GazeMonitoring.Common.Streams;
using GazeMonitoring.Model;

namespace MockMonitoring.GazeStreams {
    public abstract class BaseMockGazeStream : GazePointStream {
        private static readonly Random _random = new Random();
        protected BaseMockGazeStream() {
            InitStream();
        }

        private void InitStream() {
            Task.Factory.StartNew(() => {
                while (true) {
                    OnGazePointReceived(new GazePointReceivedEventArgs {
                        GazePoint = new GazePoint {
                            X = _random.NextDouble(),
                            Y = _random.NextDouble(),
                            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                        }
                    });
                    Thread.Sleep(5);
                }
            });
        }
    }
}
