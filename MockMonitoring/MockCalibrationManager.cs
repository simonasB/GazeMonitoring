using System.Collections.Generic;
using System.Threading.Tasks;
using GazeMonitoring.EyeTracker.Core.Calibration;

namespace MockMonitoring
{
    public class MockCalibrationManager : ICalibrationManager
    {
        private readonly List<string> _profiles;
        private string _currentProfile;

        public MockCalibrationManager()
        {
            _profiles = new List<string>
            {
                "profile1",
                "profile2",
                "profile3"
            };
            _currentProfile = _profiles[0];
        }

        public async Task CreateProfileAsync(string profileName)
        {
            _profiles.Add(profileName);
        }

        public async Task SetCurrentProfileAsync(string profileName)
        {
            _currentProfile = profileName;
        }

        public async Task RenameProfileAsync(string oldProfileName, string newProfileName)
        {
            var index = _profiles.FindIndex(o => o == oldProfileName);
            _profiles[index] = newProfileName;
        }

        public async Task DeleteProfileAsync(string profileName)
        {
            _profiles.Remove(profileName);
        }

        public async Task Calibrate(string profileName)
        {
        }

        public async Task<List<string>> GetProfilesAsync()
        {
            return _profiles;
        }

        public async Task<string> GetCurrentProfileAsync()
        {
            return _currentProfile;
        }
    }
}
