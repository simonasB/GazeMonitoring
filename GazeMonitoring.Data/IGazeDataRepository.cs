using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data {
    public interface IGazeDataRepository : IDisposable
    {
        void SaveGazePoint(GazePoint gazePoint);
        void SaveSaccade(Saccade saccade);
        void SaveFixationPoint(FixationPoint point);
    }
}
