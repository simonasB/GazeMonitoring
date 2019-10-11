using System.ComponentModel;
using System.Runtime.CompilerServices;
using GazeMonitoring.Base;
using GazeMonitoring.Unmanaged;

namespace GazeMonitoring.ViewModels
{
    public class OptionsViewModel : INotifyPropertyChanged
    {
        private readonly IGlobalHotKeyManager _globalHotKeyManager;
        private Hotkey _captureScreenRegionHotkey;
        private Hotkey _createScreenConfigurationHotkey;
        private Hotkey _editScreenConfigurationHotkey;

        public OptionsViewModel(IGlobalHotKeyManager globalHotKeyManager)
        {
            _globalHotKeyManager = globalHotKeyManager;
        }

        public Hotkey CaptureScreenRegionHotkey
        {
            get => _captureScreenRegionHotkey;
            set
            {
                if (_captureScreenRegionHotkey != value)
                {
                    _captureScreenRegionHotkey = value;
                    _globalHotKeyManager.ChangeGlobalHotKey(EGlobalHotKey.CreateScreenConfiguration, _captureScreenRegionHotkey.Key, _captureScreenRegionHotkey.Modifiers);
                    OnPropertyChanged();
                }
            }
        }

        public Hotkey StartGazeRecordingHotkey { get; set; }

        public Hotkey StopGazeRecordingHotkey { get; set; }

        public Hotkey CreateScreenConfigurationHotkey
        {
            get => _createScreenConfigurationHotkey;
            set
            {
                if (_createScreenConfigurationHotkey != value)
                {
                    _createScreenConfigurationHotkey = value;
                    _globalHotKeyManager.ChangeGlobalHotKey(EGlobalHotKey.CreateScreenConfiguration, _captureScreenRegionHotkey.Key, _captureScreenRegionHotkey.Modifiers);
                    OnPropertyChanged();
                }
            }
        }

        public Hotkey EditScreenConfigurationHotkey
        {
            get => _editScreenConfigurationHotkey;
            set
            {
                if (_editScreenConfigurationHotkey != value)
                {
                    _editScreenConfigurationHotkey = value;
                    _globalHotKeyManager.ChangeGlobalHotKey(EGlobalHotKey.EditScreenConfiguration, _captureScreenRegionHotkey.Key, _captureScreenRegionHotkey.Modifiers);
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
