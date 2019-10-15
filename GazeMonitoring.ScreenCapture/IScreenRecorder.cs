using GazeMonitoring.Model;

namespace GazeMonitoring.ScreenCapture {
    public interface IScreenRecorder {
        void StartRecording(RecorderParams recorderParams, IMonitoringContext monitoringContext);
        void StopRecording();
    }
}
