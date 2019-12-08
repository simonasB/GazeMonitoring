using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GazeMonitoring.Balloon;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.Logging;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using GazeMonitoring.Model;
using GazeMonitoring.Monitor;
using GazeMonitoring.ScreenCapture;
using GazeMonitoring.WindowModels;
using Hardcodet.Wpf.TaskbarNotification;

namespace GazeMonitoring.ViewModels {
    public class SessionViewModel : INotifyPropertyChanged, IMainSubViewModel {
        private bool _isAnonymous;
        private bool _isScreenRecorded;
        private bool _isStarted;
        private bool _isBusy;
        private readonly IBalloonService _balloonService;
        private readonly IGazeDataMonitorFactory _gazeDataMonitorFactory;
        private readonly IScreenRecorder _screenRecorder;
        private readonly IMessenger _messenger;
        private readonly IAppLocalContextManager _appLocalContextManager;
        private SubjectInfo _subjectInfo;
        private readonly IEyeTrackerStatusProvider _eyeTrackerStatusProvider;
        private readonly IGazeDataMonitorFinalizer _gazeDataMonitorFinalizer;
        private const int PollIntervalSeconds = 5;
        private readonly ILogger _logger;
        private IGazeDataMonitor _gazeDataMonitor;

        public SessionViewModel(IBalloonService balloonService, IGazeDataMonitorFactory gazeDataMonitorFactory, IEyeTrackerStatusProvider eyeTrackerStatusProvider, IGazeDataMonitorFinalizer gazeDataMonitorFinalizer, IScreenRecorder screenRecorder, ILoggerFactory loggerFactory, IMessenger messenger, IAppLocalContextManager appLocalContextManager) {
            _balloonService = balloonService;
            _gazeDataMonitorFactory = gazeDataMonitorFactory;
            StartCommand = new RelayCommand(OnStart, CanStart);
            StopCommand = new AwaitableDelegateCommand(OnStop, CanStop);
            SettingsCommand = new RelayCommand(OnSettings);
            SubjectInfoWindowModel = new SubjectInfoWindowModel();
            _eyeTrackerStatusProvider = eyeTrackerStatusProvider;
            _gazeDataMonitorFinalizer = gazeDataMonitorFinalizer;
            _screenRecorder = screenRecorder;
            _messenger = messenger;
            _appLocalContextManager = appLocalContextManager;
            InvokeEyeTrackerStatusPolling();
            EyeTrackerStatusWindowModel = new EyeTrackerStatusWindowModel(StartCommand, StopCommand) {
                EyeTrackerName = CommonConstants.DefaultEyeTrackerName
            };
            _logger = loggerFactory.GetLogger(typeof(SessionViewModel));
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
                    SubjectInfoWindowModel.Name = null;
                    SubjectInfoWindowModel.Age = 0;
                    SubjectInfoWindowModel.Age = null;
                    SubjectInfoWindowModel.Details = null;
                    SubjectInfoWindowModel.ResetErrors();
                }
            }
        }

        public bool IsScreenRecorded {
            get => _isScreenRecorded;
            set
            {
                if (_isScreenRecorded != value) {
                    _isScreenRecorded = value;
                    OnPropertyChanged();
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

        public SubjectInfoWindowModel SubjectInfoWindowModel { get; set; }

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

        public RelayCommand StartCommand { get; }

        public RelayCommand SettingsCommand { get; }

        public AwaitableDelegateCommand StopCommand { get; }

        public EMainSubViewModel SubViewModel => EMainSubViewModel.SessionViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanStop() {
            return IsStarted && !IsBusy && EyeTrackerStatusWindowModel.IsAvailable;
        }

        private async Task OnStop() {
            IsBusy = true;
            try {
                await Task.Run(() => {
                    _gazeDataMonitor.Stop();
                    _gazeDataMonitor = null;
                    _subjectInfo.SessionEndTimeStamp = DateTime.UtcNow;
                });


                var finalizationTask = Task.Run(() =>
                {
                    _gazeDataMonitorFinalizer.FinalizeMonitoring(new MonitoringContext
                        {SubjectInfo = _subjectInfo, DataStream = SubjectInfoWindowModel.DataStream});
                });

                var stopRecordingTask = Task.Run(() => {
                    if (IsScreenRecorded) {
                        _screenRecorder?.StopRecording();
                    }
                });

                await Task.WhenAll(finalizationTask, stopRecordingTask);
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

        private void OnStart() {
            if (!IsFormValid()) {
                return;
            }

            IsBusy = true;

            try {
                _subjectInfo = new SubjectInfo {
                    SessionId = Guid.NewGuid().ToString(),
                    SessionStartTimestamp = DateTime.UtcNow
                };

                var dataFolderName = DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_fff", CultureInfo.InvariantCulture);

                if (!IsAnonymous) {
                    _subjectInfo.Name = SubjectInfoWindowModel.Name;
                    _subjectInfo.Age = SubjectInfoWindowModel.Age;
                    _subjectInfo.Details = SubjectInfoWindowModel.Details;
                    dataFolderName = $"{_subjectInfo.Name}_{_subjectInfo.Age}_{_subjectInfo.Details}:{dataFolderName}";
                }

                var dataFilesPath = Path.Combine(_appLocalContextManager.Get().DataFilesPath, dataFolderName);

                if (!Directory.Exists(dataFilesPath))
                    Directory.CreateDirectory(dataFilesPath);

                var monitoringContext = new MonitoringContext
                    {SubjectInfo = _subjectInfo, DataStream = SubjectInfoWindowModel.DataStream, DataFilesPath = dataFilesPath };
                _gazeDataMonitor = _gazeDataMonitorFactory.Create(monitoringContext);
                _gazeDataMonitor.Start();

                if (IsScreenRecorded) {
                    _screenRecorder.StartRecording(
                        new RecorderParams(
                            Path.Combine(dataFilesPath, $"video_{SubjectInfoWindowModel.DataStream}_{DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_fff", CultureInfo.InvariantCulture)}.avi"),
                            10, 50), monitoringContext);
                }
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

                if (string.IsNullOrEmpty(SubjectInfoWindowModel.Name)) {
                    SubjectInfoWindowModel.Name = null;
                    isFormValid = false;
                }

                if (SubjectInfoWindowModel.Age == null) {
                    SubjectInfoWindowModel.Age = null;
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
