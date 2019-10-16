using System;

namespace GazeMonitoring.Monitor {
    public interface IGazeDataMonitor : IDisposable {
        void Start();
        void Stop();
    }
}