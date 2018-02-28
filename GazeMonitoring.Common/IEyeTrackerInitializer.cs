using System;

namespace GazeMonitoring.Common {
    public interface IEyeTrackerInitializer : IDisposable {
        void Initialize();
    }
}
