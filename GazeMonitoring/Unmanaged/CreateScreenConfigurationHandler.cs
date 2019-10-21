using GazeMonitoring.Base;
using GazeMonitoring.DataAccess;
using GazeMonitoring.DataAccess.LiteDB;
using GazeMonitoring.Views;

namespace GazeMonitoring.Unmanaged
{
    public class CreateScreenConfigurationHandler : IGlobalHotKeyHandler
    {
        private readonly IAppLocalContextManager _appLocalContextManager;
        private readonly IConfigurationRepository _configurationRepository;

        public CreateScreenConfigurationHandler(IAppLocalContextManager appLocalContextManager, IConfigurationRepository configurationRepository)
        {
            _appLocalContextManager = appLocalContextManager;
            _configurationRepository = configurationRepository;
        }

        public void Handle()
        {
            var window = new ScreenConfigurationWindow(_appLocalContextManager, _configurationRepository);
            window.Show();
        }
    }
}
