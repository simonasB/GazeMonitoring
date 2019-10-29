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

        public MonitoringConfigurationsViewModel(IConfigurationRepository configurationRepository, IMessenger messenger)
        {
            _configurationRepository = configurationRepository;
            _messenger = messenger;
            MonitoringConfigurations = new ObservableCollection<MonitoringConfiguration>(_configurationRepository.Search<MonitoringConfiguration>());
            EditCommand = new RelayCommand<MonitoringConfiguration>(
                monitoringConfig => { _messenger.Send(new ShowMonitoringConfigurationDetailsMessage(monitoringConfig)); });
            DeleteCommand = new RelayCommand<MonitoringConfiguration>(
                monitoringConfig =>
                {
                    _configurationRepository.Delete<MonitoringConfiguration>(monitoringConfig.Id);
                    MonitoringConfigurations.Remove(monitoringConfig);
                });
            AddCommand = new RelayCommand(() => _messenger.Send(new ShowMonitoringConfigurationAddMessage()));
        }

        public ObservableCollection<MonitoringConfiguration> MonitoringConfigurations { get; set; }

        public RelayCommand AddCommand { get; }

        public RelayCommand<MonitoringConfiguration> DeleteCommand { get; }

        public RelayCommand<MonitoringConfiguration> EditCommand { get; }

        public ESettingsSubViewModel ESettingsSubViewModel => ESettingsSubViewModel.MonitoringConfigurationsViewModel;
    }
}
