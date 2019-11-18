using System.Collections.ObjectModel;
using System.Linq;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.EyeTracker.Core.Calibration;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using GazeMonitoring.WindowModels;

namespace GazeMonitoring.ViewModels
{
    public class ProfilesViewModel : ViewModelBase, IMainSubViewModel
    {
        private readonly IMessenger _messenger;
        private readonly ICalibrationManager _calibrationManager;
        private ObservableCollection<ProfileWindowModel> _profiles;
        public EMainSubViewModel SubViewModel => EMainSubViewModel.ProfilesViewModel;

        private string _currentProfile;
        private ProfileWindowModel _selectedProfile;
        private bool _addEditProfileModeEnabled;
        private ProfileWindowModel _addEditProfileWindowModel;

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

        public ProfilesViewModel(IMessenger messenger, ICalibrationManager calibrationManager)
        {
            _messenger = messenger;
            _calibrationManager = calibrationManager;
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
        }

        public RelayCommand BackCommand => new RelayCommand(() =>
        {
            _messenger.Send(new ShowMainNavigationMessage());
        });

        public AwaitableDelegateCommand<ProfileWindowModel> CalibrateCommand => new AwaitableDelegateCommand<ProfileWindowModel>(async (profile) =>
            {
                await _calibrationManager.Calibrate(profile.Name);
            });

        public AwaitableDelegateCommand<ProfileWindowModel> DeleteCommand => new AwaitableDelegateCommand<ProfileWindowModel>(async (profile) =>
        {
            await _calibrationManager.DeleteProfileAsync(profile.Name);
            Profiles.Remove(profile);
            if (_currentProfile == profile.Name)
            {
                _messenger.Send(new ShowProfilesMessage());
            }
        });

        public AwaitableDelegateCommand SaveProfileCommand => new AwaitableDelegateCommand(async () =>
        {
            await _calibrationManager.CreateProfileAsync(AddEditProfileWindowModel.Name);
            _messenger.Send(new ShowMainNavigationMessage());
        });

        public DelegateCommand AddCommand => new DelegateCommand(() =>
        {
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
                if (value == null) return;
                SetProperty(ref _selectedProfile, value);
                _calibrationManager.SetCurrentProfileAsync(_selectedProfile.Name).Wait();
            }
        }
    }
}
