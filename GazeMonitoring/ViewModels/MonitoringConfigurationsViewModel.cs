using System.Collections.ObjectModel;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.DataAccess;
using GazeMonitoring.Model;

namespace GazeMonitoring.ViewModels
{
    public class MonitoringConfigurationsViewModel : ViewModelBase, ISettingsSubViewModel
    {
        private readonly IConfigurationRepository _configurationRepository;

        public MonitoringConfigurationsViewModel(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
            MonitoringConfigurations = new ObservableCollection<MonitoringConfiguration>(_configurationRepository.Search<MonitoringConfiguration>());
        }

        public ObservableCollection<MonitoringConfiguration> MonitoringConfigurations { get; set; }

        private RelayCommand<MonitoringConfiguration> _deleteCommand;


        public RelayCommand<MonitoringConfiguration> DeleteCommand
        {
            get
            {
                return _deleteCommand
                       ?? (_deleteCommand = new RelayCommand<MonitoringConfiguration>(
                           monitoringConfig =>
                           {
                               MonitoringConfigurations.Remove(monitoringConfig);
                           }));
            }
        }

        public ESettingsSubViewModel ESettingsSubViewModel => ESettingsSubViewModel.MonitoringConfigurationsViewModel;
    }
}
