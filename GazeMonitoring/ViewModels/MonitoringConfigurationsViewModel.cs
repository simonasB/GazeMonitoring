using System.Collections.ObjectModel;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.DataAccess;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using GazeMonitoring.Model;

namespace GazeMonitoring.ViewModels
{
    public class MonitoringConfigurationsViewModel : ViewModelBase, ISettingsSubViewModel
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMessenger _messenger;
        private readonly IAppLocalContextManager _appLocalContextManager;

        public MonitoringConfigurationsViewModel(IConfigurationRepository configurationRepository, IMessenger messenger, IAppLocalContextManager appLocalContextManager)
        {
            _configurationRepository = configurationRepository;
            _messenger = messenger;
            _appLocalContextManager = appLocalContextManager;
            MonitoringConfigurations = new ObservableCollection<MonitoringConfiguration>(_configurationRepository.Search<MonitoringConfiguration>());
            EditCommand = new RelayCommand<MonitoringConfiguration>(
                monitoringConfig =>
                {
                    _appLocalContextManager.SetMonitoringConfigurationId(monitoringConfig.Id);
                    _messenger.Send(new ShowEditMonitoringConfigurationMessage(monitoringConfig));
                });
            DeleteCommand = new RelayCommand<MonitoringConfiguration>(
                monitoringConfig =>
                {
                    _configurationRepository.Delete<MonitoringConfiguration>(monitoringConfig.Id);
                    MonitoringConfigurations.Remove(monitoringConfig);
                });
            AddCommand = new RelayCommand(() =>
            {
                _appLocalContextManager.SetMonitoringConfigurationId(null);
                _messenger.Send(new ShowAddMonitoringConfigurationMessage());
            });
            _messenger.Register<SettingsSubViewModelChangedMessage>(message =>
            {
                if (message.SettingsSubViewModel == ESettingsSubViewModel.MonitoringConfigurationsViewModel)
                {
                    MonitoringConfigurations = new ObservableCollection<MonitoringConfiguration>(_configurationRepository.Search<MonitoringConfiguration>());
                }
            });
        }

        public ObservableCollection<MonitoringConfiguration> MonitoringConfigurations { get; set; }

        public RelayCommand AddCommand { get; }

        public RelayCommand<MonitoringConfiguration> DeleteCommand { get; }

        public RelayCommand<MonitoringConfiguration> EditCommand { get; }

        public ESettingsSubViewModel ESettingsSubViewModel => ESettingsSubViewModel.MonitoringConfigurationsViewModel;
    }
}
