using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GazeMonitoring.Balloon;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.DataAccess;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.IO;
using GazeMonitoring.Logging;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using GazeMonitoring.Model;
using GazeMonitoring.Monitor;
using GazeMonitoring.ScreenCapture;
using GazeMonitoring.WindowModels;
using Hardcodet.Wpf.TaskbarNotification;

namespace GazeMonitoring.ViewModels {
    public class SessionViewModel : ViewModelBase, IMainSubViewModel {
        private bool _isAnonymous;
        private bool _isStarted;
        private bool _isBusy;
        private readonly IBalloonService _balloonService;
        private readonly IGazeDataMonitorFactory _gazeDataMonitorFactory;
        private readonly IMessenger _messenger;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IDataFolderManager _dataFolderManager;
        private SubjectInfo _subjectInfo;
        private readonly IEyeTrackerStatusProvider _eyeTrackerStatusProvider;
        private const int PollIntervalSeconds = 5;
        private readonly ILogger _logger;
        private IGazeDataMonitor _gazeDataMonitor;

        [Obsolete("Only for design data", true)]
        public SessionViewModel() : this(null, null, null, null, null, null, null)
        {
        }

        public SessionViewModel(IBalloonService balloonService, IGazeDataMonitorFactory gazeDataMonitorFactory, IEyeTrackerStatusProvider eyeTrackerStatusProvider, ILoggerFactory loggerFactory, IMessenger messenger, IConfigurationRepository configurationRepository, IDataFolderManager dataFolderManager) {
            _balloonService = balloonService;
            _gazeDataMonitorFactory = gazeDataMonitorFactory;
            StartCommand = new AwaitableDelegateCommand(OnStart, CanStart);
            StopCommand = new AwaitableDelegateCommand(OnStop, CanStop);
            SettingsCommand = new RelayCommand(OnSettings);
            SessionWindowModel = new SessionWindowModel();
            _eyeTrackerStatusProvider = eyeTrackerStatusProvider;
            _messenger = messenger;
            _configurationRepository = configurationRepository;
            _dataFolderManager = dataFolderManager;
            InvokeEyeTrackerStatusPolling();
            EyeTrackerStatusWindowModel = new EyeTrackerStatusWindowModel(StartCommand, StopCommand) {
                EyeTrackerName = CommonConstants.DefaultEyeTrackerName
            };
            _logger = loggerFactory.GetLogger(typeof(SessionViewModel));
            _messenger.Register<ShowStartNewSessionMessage>(_ =>
            {
                var defaultConfiguration = new MonitoringConfiguration
                {
                    Name = "None"
                };
                SessionWindowModel.MonitoringConfigurations = new List<MonitoringConfiguration>
                {
                    defaultConfiguration
                };
                SessionWindowModel.MonitoringConfigurations.AddRange(_configurationRepository.Search<MonitoringConfiguration>());
                SessionWindowModel.SelectedMonitoringConfiguration = defaultConfiguration;
            });
        }

        public bool IsAnonymous {
            get => _isAnonymous;
            set
            {
                if (_isAnonymous != value) {
                    _isAnonymous = value;
                    OnPropertyChanged();
                }

                if (value) {
                    SessionWindowModel.Name = null;
                    SessionWindowModel.Age = 0;
                    SessionWindowModel.Age = null;
                    SessionWindowModel.Details = null;
                    SessionWindowModel.ResetErrors();
                }
            }
        }

        public bool IsStarted {
            get => _isStarted;
            set
            {
                if (_isStarted != value) {
                    _isStarted = value;
                    OnPropertyChanged();
                    StartCommand.RaiseCanExecuteChanged();
                    StopCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public SessionWindowModel SessionWindowModel { get; set; }

        public bool IsBusy {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value) {
                    _isBusy = value;
                    OnPropertyChanged();
                    StartCommand.RaiseCanExecuteChanged();
                    StopCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public EyeTrackerStatusWindowModel EyeTrackerStatusWindowModel { get; set; }

        public AwaitableDelegateCommand StartCommand { get; }

        public RelayCommand SettingsCommand { get; }

        public AwaitableDelegateCommand StopCommand { get; }

        public EMainSubViewModel SubViewModel => EMainSubViewModel.SessionViewModel;

        private bool CanStop() {
            return IsStarted && !IsBusy && EyeTrackerStatusWindowModel.IsAvailable;
        }

        private async Task OnStop() {
            IsBusy = true;
            try {
                await Task.Run(async () => {
                    await _gazeDataMonitor.StopAsync().ConfigureAwait(false);
                    _gazeDataMonitor = null;
                    _subjectInfo.SessionEndTimeStamp = DateTime.UtcNow;
                });
            } catch (Exception ex) {
                _logger.Error($"Unhandled error occured on stop. Ex: {ex}");
                ShowErrorBalloon();
            }

            IsBusy = false;
            IsStarted = false;
        }

        private bool CanStart() {
            return !IsStarted && !IsBusy && EyeTrackerStatusWindowModel.IsAvailable;
        }

        private async Task OnStart() {
            if (!IsFormValid()) {
                return;
            }

            IsBusy = true;

            try {
                _subjectInfo = new SubjectInfo {
                    SessionId = Guid.NewGuid().ToString(),
                    SessionStartTimestamp = DateTime.UtcNow
                };

                if (!IsAnonymous) {
                    _subjectInfo.Name = SessionWindowModel.Name;
                    _subjectInfo.Age = SessionWindowModel.Age;
                    _subjectInfo.Details = SessionWindowModel.Details;
                }

                var dataFilesPath = _dataFolderManager.GetDataFilesPath(_subjectInfo, IsAnonymous);

                var monitoringContext = new MonitoringContext
                {
                    SubjectInfo = _subjectInfo,
                    DataStream = SessionWindowModel.DataStream,
                    DataFilesPath = dataFilesPath,
                    IsAnonymous = IsAnonymous,
                    IsScreenRecorded = SessionWindowModel.IsReportGenerated
                };
                _gazeDataMonitor = _gazeDataMonitorFactory.Create(monitoringContext);
                await _gazeDataMonitor.StartAsync();
            } catch (Exception ex){
                _logger.Error($"Unhandled exception occured on start. {ex}");
                ShowErrorBalloon();
                IsBusy = false;
                return;
            }

            IsBusy = false;
            IsStarted = true;
        }

        private bool IsFormValid() {
            var isFormValid = true;

            if (!IsAnonymous) {

                if (string.IsNullOrEmpty(SessionWindowModel.Name)) {
                    SessionWindowModel.Name = null;
                    isFormValid = false;
                }

                if (SessionWindowModel.Age == null) {
                    SessionWindowModel.Age = null;
                    isFormValid = false;
                }
            }

            return isFormValid;
        }

        private void ShowErrorBalloon() {
            _balloonService.ShowBalloonTip("Error", "Unrecoverable error occurred.", BalloonIcon.Error);
        }

        private void InvokeEyeTrackerStatusPolling() {
            Task.Factory.StartNew(async () => {

                while (true) {
                    var status = new EyeTrackerStatus();
                    try {
                        status = await _eyeTrackerStatusProvider.GetStatusAsync();
                    } catch {
                        status.IsAvailable = false;
                        status.Name = CommonConstants.DefaultEyeTrackerName;
                    }
                    await Application.Current.Dispatcher.BeginInvoke(new Action(async () => {
                        EyeTrackerStatusWindowModel.IsAvailable = status.IsAvailable;
                        EyeTrackerStatusWindowModel.EyeTrackerName = EyeTrackerStatusWindowModel.IsAvailable ? status.Name : CommonConstants.DefaultEyeTrackerName;
                        if (!status.IsAvailable && IsStarted) {
                            await OnStop();
                        }
                    }));

                    await Task.Delay(TimeSpan.FromSeconds(PollIntervalSeconds));
                }
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public RelayCommand BackCommand => new RelayCommand(() =>
        {
            _messenger.Send(new ShowMainNavigationMessage());
        });

        private void OnSettings()
        {
            _messenger.Send(new ShowSettingsMessage());
        }
    }
}
