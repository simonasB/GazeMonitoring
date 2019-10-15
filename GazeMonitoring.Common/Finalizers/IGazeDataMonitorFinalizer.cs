using GazeMonitoring.Model;

namespace GazeMonitoring.Common.Finalizers {
    public interface IGazeDataMonitorFinalizer {
        void FinalizeMonitoring(IMonitoringContext monitoringContext);
    }
}
