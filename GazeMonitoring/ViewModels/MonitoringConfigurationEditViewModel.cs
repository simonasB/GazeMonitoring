using System.Collections.ObjectModel;
using System.Linq;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.DataAccess;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using GazeMonitoring.Model;

namespace GazeMonitoring.ViewModels
{
    public class MonitoringConfigurationEditViewModel : ViewModelBase, ISettingsSubViewModel
    {
        private readonly IMessenger _messenger;
        private readonly IConfigurationRepository _configurationRepository;

        public ObservableCollection<ScreenConfiguration> ScreenConfigurations { get; set; }

        private MonitoringConfiguration _monitoringConfiguration;

        public MonitoringConfigurationEditViewModel(IMessenger messenger, IConfigurationRepository configurationRepository)
        {
            _messenger = messenger;
            _configurationRepository = configurationRepository;
            _messenger.Register<ShowMonitoringConfigurationDetailsMessage>(o =>
            {
                ScreenConfigurations = new ObservableCollection<ScreenConfiguration>(o.MonitoringConfiguration.ScreenConfigurations);
                _monitoringConfiguration = o.MonitoringConfiguration;
            });
        }

        public ESettingsSubViewModel ESettingsSubViewModel => ESettingsSubViewModel.MonitoringConfigurationEditViewModel;

        private RelayCommand<ScreenConfiguration> _deleteCommand;

        public RelayCommand<ScreenConfiguration> DeleteCommand
        {
            get
            {
                return _deleteCommand
                       ?? (_deleteCommand = new RelayCommand<ScreenConfiguration>(
                           screenConfiguration =>
                           {
                               ScreenConfigurations.Remove(screenConfiguration);
                               var screenConfigToDelete = _monitoringConfiguration.ScreenConfigurations.FirstOrDefault(o => o.Id == screenConfiguration.Id);

                               if (screenConfigToDelete == null) return;

                               _monitoringConfiguration.ScreenConfigurations.Remove(screenConfigToDelete);
                               _configurationRepository.Update(_monitoringConfiguration);
                           }));
            }
        }

        private RelayCommand<ScreenConfiguration> _editCommand;

        public RelayCommand<ScreenConfiguration> EditCommand
        {
            get
            {
                return _editCommand
                       ?? (_editCommand = new RelayCommand<ScreenConfiguration>(
                           monitoringConfig =>
                           {
                               ScreenConfigurations.Remove(monitoringConfig);
                           }));
            }
        }
    }
}
