using GazeMonitoring.Base;
using GazeMonitoring.DataAccess;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;

namespace GazeMonitoring.Views
{
    public interface IScreenConfigurationWindowHandler
    {
        void Handle();
    }

    public class ScreenConfigurationWindowHandler : IScreenConfigurationWindowHandler
    {
        private readonly IAppLocalContextManager _appLocalContextManager;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMessenger _messenger;

        public ScreenConfigurationWindowHandler(IAppLocalContextManager appLocalContextManager, IConfigurationRepository configurationRepository, IMessenger messenger)
        {
            _appLocalContextManager = appLocalContextManager;
            _configurationRepository = configurationRepository;
            _messenger = messenger;
        }

        public void Handle()
        {
            _messenger.Register<ShowCreateScreenConfigurationMessage>(_ =>
            {
                var window = new ScreenConfigurationWindow(_appLocalContextManager, _configurationRepository, true);
                window.Show();
            });

            _messenger.Register<ShowEditScreenConfigurationMessage>(_ =>
            {
                var window = new ScreenConfigurationWindow(_appLocalContextManager, _configurationRepository, false);
                window.Show();
            });
        }
    }
}
