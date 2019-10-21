using GazeMonitoring.Base;

namespace GazeMonitoring.ViewModels
{
    public class MonitoringConfigurationsViewModel : ViewModelBase, ISettingsSubViewModel
    {
        public ESettingsSubViewModel ESettingsSubViewModel => ESettingsSubViewModel.MonitoringConfigurationsViewModel;
    }
}
