using System.Linq;
using GazeMonitoring.Base;
using GazeMonitoring.DataAccess;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using GazeMonitoring.Model;

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
                if (!IsMonitoringConfigurationContextValid(out var _))
                {
                    _messenger.Send(new ShowSettingsMessage());
                    _messenger.Send(new ShowMonitoringConfigurationsMessage());
                }
                else
                {
                    var window = new ScreenConfigurationWindow(_appLocalContextManager, _configurationRepository, _messenger, true);
                    window.Show();
                }
            });

            _messenger.Register<ShowEditScreenConfigurationMessage>(_ =>
            {
                if (!IsMonitoringConfigurationContextValid(out var monitoringConfiguration))
                {
                    _messenger.Send(new ShowSettingsMessage());
                    _messenger.Send(new ShowMonitoringConfigurationsMessage());
                }
                else if (monitoringConfiguration.ScreenConfigurations?.FirstOrDefault(o =>
                             o.Id == _appLocalContextManager.Get().ScreenConfigurationId) == null)
                {
                    _messenger.Send(new ShowSettingsMessage());
                    _messenger.Send(new ShowEditMonitoringConfigurationMessage(monitoringConfiguration));
                }
                else
                {
                    var window = new ScreenConfigurationWindow(_appLocalContextManager, _configurationRepository,
                        _messenger, false);
                    window.Show();
                }
            });
        }

        private bool IsMonitoringConfigurationContextValid(out MonitoringConfiguration monitoringConfiguration)
        {
            monitoringConfiguration = null;
            var context = _appLocalContextManager.Get();
            if (context.MonitoringConfigurationId == null)
            {
                return false;
            }

            monitoringConfiguration = _configurationRepository.Search<MonitoringConfiguration>(context.MonitoringConfigurationId.Value);
            return monitoringConfiguration != null;
        }
    }
}
