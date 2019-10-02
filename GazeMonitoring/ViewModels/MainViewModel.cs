using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.Logging;
using GazeMonitoring.Model;
using GazeMonitoring.ScreenCapture;
using GazeMonitoring.Views;
using GazeMonitoring.Wrappers;
using Hardcodet.Wpf.TaskbarNotification;
using IContainer = Autofac.IContainer;

namespace GazeMonitoring.ViewModels {
    public class MainViewModel : INotifyPropertyChanged {
        private bool _isAnonymous;
        private bool _isScreenRecorded;
        private bool _isStarted;
        private bool _isBusy;
        private IGazeDataMonitor _gazeDataMonitor;
        private readonly IContainer _container;
        private readonly IBalloonService _balloonService;
        private static ILifetimeScope _lifetimeScope;
        private IScreenRecorder _screenRecorder;
        private SubjectInfo _subjectInfo;
        private readonly IEyeTrackerStatusProvider _eyeTrackerStatusProvider;
        private const int PollIntervalSeconds = 5;
        private readonly ILogger _logger;

        public MainViewModel(IContainer container, IBalloonService balloonService) {
            _container = container;
            _balloonService = balloonService;
            StartCommand = new RelayCommand(OnStart, CanStart);
            CaptureCommand = new RelayCommand(OnCapture, CanCapture);
            StopCommand = new AwaitableDelegateCommand(OnStop, CanStop);
            OptionsCommand = new RelayCommand(() =>
            {
                var screenConfigurationWindow = new OptionsWindow();
                screenConfigurationWindow.Show();
            });
            SubjectInfoWrapper = new SubjectInfoWrapper();
            _eyeTrackerStatusProvider = _container.Resolve<IEyeTrackerStatusProvider>();
            InvokeEyeTrackerStatusPolling();
            EyeTrackerStatusWrapper = new EyeTrackerStatusWrapper(StartCommand, StopCommand) {
                EyeTrackerName = CommonConstants.DefaultEyeTrackerName
            };
            _logger = container.Resolve<ILoggerFactory>().GetLogger(typeof(MainViewModel));
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
                    SubjectInfoWrapper.Name = null;
                    SubjectInfoWrapper.Age = 0;
                    SubjectInfoWrapper.Age = null;
                    SubjectInfoWrapper.Details = null;
                    SubjectInfoWrapper.ResetErrors();
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

        public SubjectInfoWrapper SubjectInfoWrapper { get; set; }

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

        public EyeTrackerStatusWrapper EyeTrackerStatusWrapper { get; set; }

        public RelayCommand StartCommand { get; }

        public RelayCommand CaptureCommand { get; }

        public RelayCommand OptionsCommand { get; }

        public AwaitableDelegateCommand StopCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanStop() {
            return IsStarted && !IsBusy && EyeTrackerStatusWrapper.IsAvailable;
        }

        private async Task OnStop() {
            IsBusy = true;
            try {
                await Task.Run(() => {
                    _gazeDataMonitor.Stop();
                    _lifetimeScope.Dispose();
                    _subjectInfo.SessionEndTimeStamp = DateTime.UtcNow;
                });
                

                var finalizationTask = Task.Run(() => {
                    using (var lifetimeScope = _container.BeginLifetimeScope()) {
                        var finalizer = lifetimeScope.Resolve<IGazeDataMonitorFinalizer>(
                            new NamedParameter(Constants.DataStreamParameterName, SubjectInfoWrapper.DataStream),
                            new NamedParameter(Constants.SubjectInfoParameterName, _subjectInfo));
                        finalizer.FinalizeMonitoring();
                    }
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
            return !IsStarted && !IsBusy && EyeTrackerStatusWrapper.IsAvailable;
        }

        private void OnStart() {
            if (!IsFormValid()) {
                return;
            }

            IsBusy = true;

            try {
                _lifetimeScope = _container.BeginLifetimeScope();

                _subjectInfo = new SubjectInfo {
                    SessionId = Guid.NewGuid().ToString(),
                    SessionStartTimestamp = DateTime.UtcNow
                };

                if (!IsAnonymous) {
                    _subjectInfo.Name = SubjectInfoWrapper.Name;
                    _subjectInfo.Age = SubjectInfoWrapper.Age;
                    _subjectInfo.Details = SubjectInfoWrapper.Details;
                }

                _gazeDataMonitor = _lifetimeScope.Resolve<IGazeDataMonitor>(
                    new NamedParameter(Constants.DataStreamParameterName, SubjectInfoWrapper.DataStream),
                    new NamedParameter(Constants.SubjectInfoParameterName, _subjectInfo));
                _gazeDataMonitor.Start();

                if (IsScreenRecorded) {
                    const string videoFolderName = "recorded_videos";
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), videoFolderName))) {
                        Directory.CreateDirectory(videoFolderName);
                    }
                    _screenRecorder = _lifetimeScope.Resolve<IScreenRecorder>(
                        new NamedParameter(Constants.DataStreamParameterName, SubjectInfoWrapper.DataStream),
                        new NamedParameter(Constants.RecorderParamsParameterName,
                            new RecorderParams($"{videoFolderName}/video_{SubjectInfoWrapper.DataStream}_{DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_fff", CultureInfo.InvariantCulture)}.avi", 10,
                                50)));
                    _screenRecorder.StartRecording();
                }
            } catch (Exception ex){
                _logger.Error($"Unhandled exception occured on start. {ex}");
                ShowErrorBalloon();
                IsBusy = false;
                _lifetimeScope.Dispose();
                return;
            }

            IsBusy = false;
            IsStarted = true;
        }

        private bool IsFormValid() {
            var isFormValid = true;

            if (!IsAnonymous) {

                if (string.IsNullOrEmpty(SubjectInfoWrapper.Name)) {
                    SubjectInfoWrapper.Name = null;
                    isFormValid = false;
                }

                if (SubjectInfoWrapper.Age == null) {
                    SubjectInfoWrapper.Age = null;
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
                        EyeTrackerStatusWrapper.IsAvailable = status.IsAvailable;
                        EyeTrackerStatusWrapper.EyeTrackerName = EyeTrackerStatusWrapper.IsAvailable ? status.Name : CommonConstants.DefaultEyeTrackerName;
                        if (!status.IsAvailable && IsStarted) {
                            await OnStop();
                        }
                    }));

                    await Task.Delay(TimeSpan.FromSeconds(PollIntervalSeconds));
                }
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void OnCapture()
        {
            var screenConfigurationWindow = new ScreenConfigurationWindow();
            screenConfigurationWindow.InitializeComponent();
            screenConfigurationWindow.Show();
        }

        private bool CanCapture()
        {
            return true;
        }
    }
}
