using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GazeMonitoring.EyeTracker.Core.Calibration;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring
{
    public class TobiiCalibrationManager : ICalibrationManager
    {
        private readonly Host _host;

        public TobiiCalibrationManager(Host host)
        {
            _host = host;
        }

        public async Task CreateProfileAsync(string profileName)
        {
            try
            {
                await _host.Commands.Profile.CreateAsync(profileName).ConfigureAwait(false);
            }
            catch
            {
                // Ignore
            }
            finally
            {
                await _host.Commands.Profile.SetCurrentAsync(profileName).ConfigureAwait(false);
            }

            _host.Context.LaunchConfigurationTool(ConfigurationTool.Recalibrate, _ => { });
        }

        public async Task SetCurrentProfileAsync(string profileName)
        {
            await _host.Commands.Profile.SetCurrentAsync(profileName).ConfigureAwait(false);
        }

        public async Task RenameProfileAsync(string oldProfileName, string newProfileName)
        {
            await _host.Commands.Profile.RenameAsync(oldProfileName, newProfileName).ConfigureAwait(false);
        }

        public async Task DeleteProfileAsync(string profileName)
        {
            await _host.Commands.Profile.DeleteAsync(profileName).ConfigureAwait(false);
        }

        public async Task Calibrate(string profileName)
        {
            if (profileName != null)
            {
                await _host.Commands.Profile.SetCurrentAsync(profileName).ConfigureAwait(false);
                _host.Context.LaunchConfigurationTool(ConfigurationTool.Recalibrate, _ => { });
            }

            _host.Context.LaunchConfigurationTool(ConfigurationTool.Recalibrate, _ => { });
        }

        public async Task<List<string>> GetProfilesAsync()
        {
            var result = await _host.States.GetStateAsync<List<object>>("eyeTracking.profiles").ConfigureAwait(false);

            return result.Value.Cast<string>().ToList();
        }

        public async Task<string> GetCurrentProfileAsync()
        {
            return (await _host.States.GetUserProfileNameAsync().ConfigureAwait(false)).Value;
        }
    }
}
