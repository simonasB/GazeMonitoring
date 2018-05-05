﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.Model;
using GazeMonitoring.ScreenCapture;
using IContainer = Autofac.IContainer;

namespace GazeMonitoring {
    public class MainViewModel : INotifyPropertyChanged {
        private bool _isAnonymous;
        private bool _isScreenRecorded;
        private bool _isStarted;
        private bool _isBusy;
        private GazeDataMonitor _gazeDataMonitor;
        private readonly IContainer _container;
        private static ILifetimeScope _lifetimeScope;
        private IScreenRecorder _screenRecorder;
        private SubjectInfo _subjectInfo;

        public MainViewModel(IContainer container) {
            _container = container;
            StartCommand = new RelayCommand(OnStart, CanStart);
            StopCommand = new RelayCommand(OnStop, CanStop);
            SubjectInfoWrapper = new SubjectInfoWrapper();
        }

        public bool IsAnonymous {
            get => _isAnonymous;
            set
            {
                if (_isAnonymous != value) {
                    _isAnonymous = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsScreenRecorded {
            get { return _isScreenRecorded; }
            set
            {
                if (_isScreenRecorded != value) {
                    _isScreenRecorded = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsStarted {
            get { return _isStarted; }
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

        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanStop() {
            return IsStarted && !IsBusy;
        }

        private async void OnStop() {
            _gazeDataMonitor.Stop();
            _lifetimeScope.Dispose();
            _subjectInfo.SessionEndTimeStamp = DateTime.UtcNow;

            IsBusy = true;

            await Task.Run(() => {
                using (var lifetimeScope = _container.BeginLifetimeScope())
                {
                    var finalizer = lifetimeScope.Resolve<IGazeDataMonitorFinalizer>(
                        new NamedParameter(Constants.DataStreamParameterName, SubjectInfoWrapper.DataStream),
                        new NamedParameter(Constants.SubjectInfoParameterName, _subjectInfo));
                    finalizer.FinalizeMonitoring();
                }

                if (IsScreenRecorded) {
                    _screenRecorder?.StopRecording();
                }
            });

            IsBusy = false;
            IsStarted = false;
        }

        private bool CanStart() {
            return !IsStarted && !IsBusy;
        }

        private void OnStart() {
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

            _gazeDataMonitor = _lifetimeScope.Resolve<GazeDataMonitor>(
                new NamedParameter(Constants.DataStreamParameterName, SubjectInfoWrapper.DataStream),
                new NamedParameter(Constants.SubjectInfoParameterName, _subjectInfo));
            _gazeDataMonitor.Start();

            if (IsScreenRecorded) {
                _screenRecorder = _lifetimeScope.Resolve<IScreenRecorder>(
                    new NamedParameter(Constants.DataStreamParameterName, SubjectInfoWrapper.DataStream),
                    new NamedParameter(Constants.RecorderParamsParameterName,
                        new RecorderParams($"video_{SubjectInfoWrapper.DataStream}_{DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_fff", CultureInfo.InvariantCulture)}.avi", 10,
                            50)));
                _screenRecorder.StartRecording();
            }

            IsStarted = true;
        }
    }
}
