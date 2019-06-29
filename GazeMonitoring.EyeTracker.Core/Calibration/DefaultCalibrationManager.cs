namespace GazeMonitoring.EyeTracker.Core.Calibration
{
    /// <summary>
    /// Default mechanism to calibrate eyetracker, not suitable for every eyetracker
    /// </summary>
    public class DefaultCalibrationManager : ICalibrationManager
    {
        /// <summary>
        /// Calibrates eyetracker by default mechanism
        /// </summary>
        /// <param name="calibrationData">Previously calibrated data</param>
        /// <returns>Calibration data</returns>
        public byte[] Calibrate(byte[] calibrationData)
        {
            return null;
        }
    }
}
