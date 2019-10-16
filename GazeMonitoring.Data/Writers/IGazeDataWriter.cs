using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Writers {
    public interface IGazeDataWriter : IDisposable {
        void Write(GazePoint gazePoint);
    }
}
