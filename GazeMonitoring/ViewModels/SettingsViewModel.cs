using System.Collections.Generic;
using System.Linq;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.Messaging;

namespace GazeMonitoring.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly Dictionary<ESettingsSubViewModel, ISettingsSubViewModel> _viewModels;
        private ISettingsSubViewModel _currentViewModel;
        private bool _isVisible;

        public ISettingsSubViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public SettingsViewModel(IEnumerable<ISettingsSubViewModel> viewModels, IMessenger messenger)
        {
            _viewModels = viewModels.ToDictionary(o => o.ESettingsSubViewModel);
            CurrentViewModel = _viewModels[ESettingsSubViewModel.OptionsViewModel];
            ShowOptions = new RelayCommand(() => ShowView(ESettingsSubViewModel.OptionsViewModel));
            ShowMonitoringConfigurations = new RelayCommand(() => ShowView(ESettingsSubViewModel.MonitoringConfigurationsViewModel));
            messenger.Register<ShowSettingsMessage>(_ => IsVisible = true);
        }

        public RelayCommand ShowOptions { get; }

        public RelayCommand ShowMonitoringConfigurations { get; }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        private void ShowView(ESettingsSubViewModel viewModel)
        {
            CurrentViewModel = _viewModels[viewModel];
        }
    }
}
