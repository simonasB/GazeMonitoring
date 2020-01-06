using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;

namespace GazeMonitoring.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly Dictionary<ESettingsSubViewModel, ISettingsSubViewModel> _viewModels;
        private ISettingsSubViewModel _currentViewModel;
        private bool _isVisible;
        private bool _isBusy;

        public ISettingsSubViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public SettingsViewModel(IEnumerable<ISettingsSubViewModel> viewModels, IMessenger messenger)
        {
            _messenger = messenger;
            _viewModels = viewModels.ToDictionary(o => o.ESettingsSubViewModel);
            CurrentViewModel = _viewModels[ESettingsSubViewModel.OptionsViewModel];
            ShowOptions = new RelayCommand(() => ShowView(ESettingsSubViewModel.OptionsViewModel));
            ShowMonitoringConfigurations = new RelayCommand(() => ShowView(ESettingsSubViewModel.MonitoringConfigurationsViewModel));
            SetupMessageRegistrations();
        }

        public RelayCommand<ListBoxItem> MenuItemClicked => new RelayCommand<ListBoxItem>((o) =>
        {
            switch (o.Name)
            {
                case "Options":
                    ShowView(ESettingsSubViewModel.OptionsViewModel);
                    break;
                case "Screen":
                    ShowView(ESettingsSubViewModel.MonitoringConfigurationsViewModel);
                    break;
            }
        });

        public RelayCommand ShowOptions { get; }

        public RelayCommand ShowMonitoringConfigurations { get; }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private void ShowView(ESettingsSubViewModel viewModel)
        {
            _messenger.Send(new SettingsSubViewModelChangedMessage {SettingsSubViewModel = viewModel});
            CurrentViewModel = _viewModels[viewModel];
        }

        private void SetupMessageRegistrations()
        {
            _messenger.Register<ShowSettingsMessage>(_ => IsVisible = true);
            _messenger.Register<HideSettingsMessage>(_ => IsVisible = false);
            _messenger.Register<ShowEditMonitoringConfigurationMessage>(_ => ShowView(ESettingsSubViewModel.MonitoringConfigurationAddEditViewModel));
            _messenger.Register<ShowAddMonitoringConfigurationMessage>(_ => ShowView(ESettingsSubViewModel.MonitoringConfigurationAddEditViewModel));
            _messenger.Register<ShowMonitoringConfigurationsMessage>(_ => ShowView(ESettingsSubViewModel.MonitoringConfigurationsViewModel));
            _messenger.Register<IsBusyChangedMessage>(o => IsBusy = o.IsBusy);
        }
    }
}
