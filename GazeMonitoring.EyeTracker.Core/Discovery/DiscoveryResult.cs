namespace GazeMonitoring.EyeTracker.Core.Discovery {
    /// <summary>
    /// For reporting eyetracker status during discovery
    /// </summary>
    public class DiscoveryResult {
        /// <summary>
        /// Whethere eyetracker is currently active (connected)
        /// </summary>
        public bool IsActive { get; set; }
    }
}
