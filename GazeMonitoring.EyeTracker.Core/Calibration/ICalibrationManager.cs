namespace GazeMonitoring.EyeTracker.Core.Calibration
{
    /// <summary>
    /// Provides abstraction for calibrating eyetracker
    /// </summary>
    public interface ICalibrationManager
    {
        /// <summary>
        /// Calibrates eyetracker and returns serialized data which can be stored externally
        /// </summary>
        /// <param name="calibrationData">Previously calibrated data</param>
        /// <returns>Calibration data</returns>
        byte[] Calibrate(byte[] calibrationData);
    }
}
