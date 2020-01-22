using System.Collections.Generic;
using System.Threading.Tasks;

namespace GazeMonitoring.EyeTracker.Core.Calibration
{
    public interface ICalibrationManager
    {
        Task CreateProfileAsync(string profileName);
        Task SetCurrentProfileAsync(string profileName);
        Task RenameProfileAsync(string oldProfileName, string newProfileName);
        Task DeleteProfileAsync(string profileName);
        Task Calibrate(string profileName);

        Task<List<string>> GetProfilesAsync();

        Task<string> GetCurrentProfileAsync();
    }
}
