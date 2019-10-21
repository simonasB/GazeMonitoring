using System;
using GazeMonitoring.Base;
using GazeMonitoring.Unmanaged;

namespace GazeMonitoring.ViewModels
{
    public class OptionsViewModel : ViewModelBase, ISettingsSubViewModel
    {
        private readonly IGlobalHotKeyManager _globalHotKeyManager;
        private Hotkey _captureScreenRegionHotkey;
        private Hotkey _createScreenConfigurationHotkey;
        private Hotkey _editScreenConfigurationHotkey;

        [Obsolete("Only for design data", true)]
        public OptionsViewModel() : this(null)
        {
        }

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
                    _globalHotKeyManager.ChangeGlobalHotKey(EGlobalHotKey.CreateScreenConfiguration, _createScreenConfigurationHotkey.Key, _captureScreenRegionHotkey.Modifiers);
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
                    _globalHotKeyManager.ChangeGlobalHotKey(EGlobalHotKey.EditScreenConfiguration, _editScreenConfigurationHotkey.Key, _captureScreenRegionHotkey.Modifiers);
                    OnPropertyChanged();
                }
            }
        }

        public ESettingsSubViewModel ESettingsSubViewModel => ESettingsSubViewModel.OptionsViewModel;
    }
}
