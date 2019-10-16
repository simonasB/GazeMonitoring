using System;
using GazeMonitoring.Data.Writers;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;

namespace GazeMonitoring.Monitor {
    public class GazeDataMonitor : IGazeDataMonitor {
        private readonly GazePointStream _gazePointStream;
        private readonly IGazeDataWriter _gazeDataWriter;
        private bool _stopCalled;

        public GazeDataMonitor(GazePointStream gazePointStream, IGazeDataWriter gazeDataWriter) {
            if (gazePointStream == null) {
                throw new ArgumentNullException(nameof(gazePointStream));
            }
            if (gazeDataWriter == null) {
                throw new ArgumentNullException(nameof(gazeDataWriter));
            }
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
            _gazeDataWriter.Dispose();
            _stopCalled = true;
        }

        private void Dispose(bool disposing)
        {
            if (!disposing || !_stopCalled)
                return;

            Stop();
            _gazeDataWriter.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~GazeDataMonitor()
        {
            Dispose(false);
        }
    }
}
