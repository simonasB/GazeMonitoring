using GazeMonitoring.Common.Streams;
using GazeMonitoring.Common.Writers;
using GazeMonitoring.Model;

namespace GazeMonitoring.Common {
    public class GazeDataMonitor {
        private readonly GazePointStream _gazePointStream;
        private readonly IGazeDataWriter _gazeDataWriter;

        public GazeDataMonitor(GazePointStream gazePointStream, IGazeDataWriter gazeDataWriter) {
            _gazePointStream = gazePointStream;
            _gazeDataWriter = gazeDataWriter;
        }

        public void Start() {
            _gazePointStream.GazePointReceived += OnGazePointReceived;
        }

        private void OnGazePointReceived(object sender, GazePointReceivedEventArgs args) {
            _gazeDataWriter.Write(args.GazePoint);
        }

        public void Stop() {
            _gazePointStream.GazePointReceived -= OnGazePointReceived;
        }
    }
}
