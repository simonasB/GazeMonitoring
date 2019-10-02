using GazeMonitoring.Base;

namespace GazeMonitoring.ViewModels
{
    public class OptionsViewModel
    {
        public OptionsViewModel()
        {
            
        }

        public Hotkey CaptureScreenRegionHotkey { get; set; }
        public Hotkey StartGazeRecordingHotkey { get; set; }
        public Hotkey StopGazeRecordingHotkey { get; set; }
        public Hotkey CreateScreenConfigurationHotkey { get; set; }
        public Hotkey EditScreenConfigurationHotkey { get; set; }
    }
}
