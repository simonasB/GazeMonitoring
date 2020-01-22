using System.Collections.Generic;
using System.Threading.Tasks;

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

        public Task CreateProfileAsync(string profileName)
        {
            throw new System.NotImplementedException();
        }

        public Task SetCurrentProfileAsync(string profileName)
        {
            throw new System.NotImplementedException();
        }

        public Task RenameProfileAsync(string oldProfileName, string newProfileName)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteProfileAsync(string profileName)
        {
            throw new System.NotImplementedException();
        }

        public Task Calibrate(string profileName)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<string>> GetProfilesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetCurrentProfileAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
