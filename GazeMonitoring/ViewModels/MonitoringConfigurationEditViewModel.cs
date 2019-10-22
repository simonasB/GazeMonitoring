using System.Collections.ObjectModel;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.Messaging;
using GazeMonitoring.Model;

namespace GazeMonitoring.ViewModels
{
    public class MonitoringConfigurationEditViewModel : ViewModelBase, ISettingsSubViewModel
    {
        private readonly IMessenger _messenger;

        public ObservableCollection<ScreenConfiguration> ScreenConfigurations { get; set; }

        public MonitoringConfigurationEditViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            _messenger.Register<ShowMonitoringConfigurationDetailsMessage>(o =>
            {
                ScreenConfigurations = new ObservableCollection<ScreenConfiguration>(o.MonitoringConfiguration.ScreenConfigurations);
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
                           monitoringConfig =>
                           {
                               ScreenConfigurations.Remove(monitoringConfig);
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
