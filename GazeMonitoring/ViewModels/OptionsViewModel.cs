using System;
using System.Runtime.CompilerServices;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.HotKeys.Global;
using GazeMonitoring.IO;

namespace GazeMonitoring.ViewModels
{
    public class OptionsViewModel : ViewModelBase, ISettingsSubViewModel
    {
        private readonly IGlobalHotKeyManager _globalHotKeyManager;
        private readonly IFolderDialogService _folderDialogService;
        private readonly IAppLocalContextManager _appLocalContextManager;
        private Hotkey _captureScreenRegionHotkey;
        private Hotkey _createScreenConfigurationHotkey;
        private Hotkey _editScreenConfigurationHotkey;
        private string _dataFilesPath;

        [Obsolete("Only for design data", true)]
        public OptionsViewModel() : this(null, null, null)
        {
        }

        public OptionsViewModel(IGlobalHotKeyManager globalHotKeyManager, IFolderDialogService folderDialogService, IAppLocalContextManager appLocalContextManager)
        {
            _globalHotKeyManager = globalHotKeyManager;
            _folderDialogService = folderDialogService;
            _appLocalContextManager = appLocalContextManager;
            var createScreenConfigurationGlobalKey = _globalHotKeyManager.Get(EGlobalHotKey.CreateScreenConfiguration);
            var editScreenConfigurationGlobalKey = _globalHotKeyManager.Get(EGlobalHotKey.EditScreenConfiguration);
            _createScreenConfigurationHotkey = new Hotkey(createScreenConfigurationGlobalKey.Key, createScreenConfigurationGlobalKey.KeyModifiers);
            _editScreenConfigurationHotkey = new Hotkey(editScreenConfigurationGlobalKey.Key, editScreenConfigurationGlobalKey.KeyModifiers);
            DataFilesPath = _appLocalContextManager.Get().DataFilesPath;
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

        public RelayCommand ChangeDataFilesPathCommand => new RelayCommand(() =>
            {
                var folderPath = _folderDialogService.OpenFolderDialog();
                if (folderPath != null)
                {
                    DataFilesPath = folderPath;
                    _appLocalContextManager.SetDataFilesPath(DataFilesPath);
                }
            });

        public string DataFilesPath
        {
            get => _dataFilesPath;
            set => SetProperty(ref _dataFilesPath, value);
        }

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
