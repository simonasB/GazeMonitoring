namespace GazeMonitoring.EyeTracker.Core.Status {
    /// <summary>
    /// Describes eyetracker status
    /// </summary>
    public class EyeTrackerStatus {
        /// <summary>
        /// Whether eyetracker is available
        /// </summary>
        public bool IsAvailable { get; set; }
        
        /// <summary>
        /// Eyetracker name which displayed in the GUI
        /// </summary>
        public string Name { get; set; }
    }
}
