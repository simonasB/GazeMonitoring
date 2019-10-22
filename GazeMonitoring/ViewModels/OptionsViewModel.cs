using System;
using System.Runtime.CompilerServices;
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
            var createScreenConfigurationGlobalKey = _globalHotKeyManager.Get(EGlobalHotKey.CreateScreenConfiguration);
            var editScreenConfigurationGlobalKey = _globalHotKeyManager.Get(EGlobalHotKey.EditScreenConfiguration);
            _createScreenConfigurationHotkey = new Hotkey(createScreenConfigurationGlobalKey.Key, createScreenConfigurationGlobalKey.KeyModifiers);
            _editScreenConfigurationHotkey = new Hotkey(editScreenConfigurationGlobalKey.Key, editScreenConfigurationGlobalKey.KeyModifiers);
        }

        public Hotkey CaptureScreenRegionHotkey
        {
            get => _captureScreenRegionHotkey;
            set
            {
                if (_captureScreenRegionHotkey != value)
                {
                    _captureScreenRegionHotkey = value;
                    _globalHotKeyManager.Change(EGlobalHotKey.CreateScreenConfiguration, _captureScreenRegionHotkey.Key, _captureScreenRegionHotkey.Modifiers);
                    OnPropertyChanged();
                }
            }
        }

        public Hotkey StartGazeRecordingHotkey { get; set; }

        public Hotkey StopGazeRecordingHotkey { get; set; }

        public Hotkey CreateScreenConfigurationHotkey
        {
            get => _createScreenConfigurationHotkey;
            set => OnPropertyChanged(ref _createScreenConfigurationHotkey, value, EGlobalHotKey.CreateScreenConfiguration);
        }

        public Hotkey EditScreenConfigurationHotkey
        {
            get => _editScreenConfigurationHotkey;
            set => OnPropertyChanged(ref _editScreenConfigurationHotkey, value, EGlobalHotKey.EditScreenConfiguration);
        }

        public ESettingsSubViewModel ESettingsSubViewModel => ESettingsSubViewModel.OptionsViewModel;

        private void OnPropertyChanged(ref Hotkey currentHotkey, Hotkey updatedHotkey, EGlobalHotKey eGlobalHotKey, [CallerMemberName] string propertyName = null)
        {
            if (currentHotkey == updatedHotkey)
                return;

            currentHotkey = updatedHotkey;
            if (currentHotkey == null)
            {
                _globalHotKeyManager.Remove(eGlobalHotKey);
            }
            else
            {
                _globalHotKeyManager.Change(eGlobalHotKey, _createScreenConfigurationHotkey.Key, _createScreenConfigurationHotkey.Modifiers);
            }
            OnPropertyChanged(propertyName);
        }
    }
}
