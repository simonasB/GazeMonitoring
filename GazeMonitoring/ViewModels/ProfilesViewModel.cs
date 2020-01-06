using System;
using System.Collections.ObjectModel;
using System.Linq;
using GazeMonitoring.Balloon;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.EyeTracker.Core.Calibration;
using GazeMonitoring.Logging;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using GazeMonitoring.WindowModels;
using Hardcodet.Wpf.TaskbarNotification;

namespace GazeMonitoring.ViewModels
{
    public class ProfilesViewModel : ViewModelBase, IMainSubViewModel
    {
        private readonly IMessenger _messenger;
        private readonly ICalibrationManager _calibrationManager;
        private readonly IBalloonService _balloonService;
        private ObservableCollection<ProfileWindowModel> _profiles;
        public EMainSubViewModel SubViewModel => EMainSubViewModel.ProfilesViewModel;

        private string _currentProfile;
        private ProfileWindowModel _selectedProfile;
        private bool _addEditProfileModeEnabled;
        private ProfileWindowModel _addEditProfileWindowModel;
        private string _globalErrorMessage;
        private readonly ILogger _logger;

        public ProfileWindowModel AddEditProfileWindowModel
        {
            get => _addEditProfileWindowModel;
            set => SetProperty(ref _addEditProfileWindowModel, value);
        }

        public bool AddEditProfileModeEnabled
        {
            get => _addEditProfileModeEnabled;
            set => SetProperty(ref _addEditProfileModeEnabled, value);
        }

        public string GlobalErrorMessage
        {
            get => _globalErrorMessage;
            set => SetProperty(ref _globalErrorMessage, value);
        }

        public ProfilesViewModel(IMessenger messenger, ICalibrationManager calibrationManager, ILoggerFactory loggerFactory, IBalloonService balloonService)
        {
            _messenger = messenger;
            _calibrationManager = calibrationManager;
            _balloonService = balloonService;
            Profiles = new ObservableCollection<ProfileWindowModel>(_calibrationManager.GetProfilesAsync().Result
                .Select(o => new ProfileWindowModel {Name = o}));
            _currentProfile = _calibrationManager.GetCurrentProfileAsync().Result;
            SelectedProfile = Profiles.First(o => o.Name == _currentProfile);
            _messenger.Register<ShowProfilesMessage>(message =>
            {
                Profiles = new ObservableCollection<ProfileWindowModel>(_calibrationManager.GetProfilesAsync().Result
                    .Select(o => new ProfileWindowModel { Name = o }));
                _currentProfile = _calibrationManager.GetCurrentProfileAsync().Result;
                SelectedProfile = Profiles.First(o => o.Name == _currentProfile);
                AddEditProfileWindowModel = new ProfileWindowModel();
                AddEditProfileModeEnabled = false;
            });
            AddEditProfileWindowModel = new ProfileWindowModel();
            AddEditProfileModeEnabled = false;
            _logger = loggerFactory.GetLogger(typeof(ProfilesViewModel));
        }

        public RelayCommand BackCommand => new RelayCommand(() =>
        {
            _messenger.Send(new ShowMainNavigationMessage());
        });

        public AwaitableDelegateCommand<ProfileWindowModel> CalibrateCommand => new AwaitableDelegateCommand<ProfileWindowModel>(async (profile) =>
            {
                try
                {
                    await _calibrationManager.Calibrate(profile.Name);
                }
                catch (Exception ex)
                {
                    _balloonService.ShowBalloonTip("Error", "Failed to calibrate a profile.", BalloonIcon.Error);
                    _logger.Error($"Failed to calibrate the profile with name: '{profile.Name}'. Ex: {ex}");
                }
            });

        public AwaitableDelegateCommand<ProfileWindowModel> DeleteCommand => new AwaitableDelegateCommand<ProfileWindowModel>(async (profile) =>
        {
            try
            {
                if (profile.Name == SelectedProfile.Name)
                {
                    GlobalErrorMessage = "Cannot deleted the selected profile.";
                    return;
                }

                GlobalErrorMessage = "";
                await _calibrationManager.DeleteProfileAsync(profile.Name);
                Profiles.Remove(profile);
                if (_currentProfile == profile.Name)
                {
                    _messenger.Send(new ShowProfilesMessage());
                }
            } catch (Exception ex)
            {
                _balloonService.ShowBalloonTip("Error", "Failed to delete profile.", BalloonIcon.Error);
                _logger.Error($"Failed to save profile with name: '{AddEditProfileWindowModel.Name}'. Ex: {ex}");
            }
        });

        public AwaitableDelegateCommand SaveProfileCommand => new AwaitableDelegateCommand(async () =>
        {
            try
            {
                GlobalErrorMessage = "";

                if (!IsValidProfile()) return;

                var profiles = await _calibrationManager.GetProfilesAsync();
                if (profiles.Any(o => o == AddEditProfileWindowModel.Name))
                {
                    // TODO: Might set error to internal AddEditProfileWindowModel that error message would appear near field.
                    GlobalErrorMessage = "The profile with the same name already exists.";
                    return;
                }

                await _calibrationManager.CreateProfileAsync(AddEditProfileWindowModel.Name);
                _messenger.Send(new ShowMainNavigationMessage());
            }
            catch(Exception ex)
            {
                _balloonService.ShowBalloonTip("Error", "Failed to save profile.", BalloonIcon.Error);
                _logger.Error($"Failed to save profile with name: '{AddEditProfileWindowModel.Name}'. Ex: {ex}");
            }
        });

        public DelegateCommand AddCommand => new DelegateCommand(() =>
        {
            GlobalErrorMessage = "";
            AddEditProfileModeEnabled = true;
            AddEditProfileWindowModel = new ProfileWindowModel();
        });

        public ObservableCollection<ProfileWindowModel> Profiles
        {
            get => _profiles;
            set => SetProperty(ref _profiles, value);
        }

        public ProfileWindowModel SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                GlobalErrorMessage = "";
                if (value == null) return;
                SetProperty(ref _selectedProfile, value);
                _calibrationManager.SetCurrentProfileAsync(_selectedProfile.Name).Wait();
            }
        }

        private bool IsValidProfile()
        {
            var isValid = !AddEditProfileWindowModel.HasErrors;

            if (string.IsNullOrEmpty(AddEditProfileWindowModel.Name))
            {
                isValid = false;
                AddEditProfileWindowModel.Name = null;
            }

            return isValid;
        }
    }
}
