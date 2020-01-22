using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GazeMonitoring.Balloon;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.DataAccess;
using GazeMonitoring.IO;
using GazeMonitoring.Logging;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using GazeMonitoring.Model;
using GazeMonitoring.Powerpoint;
using GazeMonitoring.WindowModels;
using GongSolutions.Wpf.DragDrop;
using Hardcodet.Wpf.TaskbarNotification;

namespace GazeMonitoring.ViewModels
{
    public class MonitoringConfigurationAddEditViewModel : ViewModelBase, ISettingsSubViewModel, IDropTarget
    {
        private readonly IMessenger _messenger;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IAppLocalContextManager _appLocalContextManager;
        private readonly IPowerpointParser _powerpointParser;
        private readonly IFileDialogService _fileDialogService;
        private readonly IBalloonService _balloonService;

        public ObservableCollection<ScreenConfigurationWindowModel> ScreenConfigurations
        {
            get => _screenConfigurations;
            set
            {
                _screenConfigurations = value;
                OnPropertyChanged();
            }
        }

        private MonitoringConfiguration _monitoringConfiguration;
        private ObservableCollection<ScreenConfigurationWindowModel> _screenConfigurations;
        private bool _isBusy;
        private ScreenConfigurationWindowModel _selectedScreenConfiguration;
        private ScreenConfigurationWindowModel _addEditScreenConfigurationWindowModel = new ScreenConfigurationWindowModel();
        private bool _addEditScreenModeEnabled;
        private List<AreaOfInterest> _addedAreasOfInterestWithUsingHotkey;
        private readonly ILogger _logger;
        private bool _isMonitoringConfigurationSaved;

        public MonitoringConfigurationWindowModel MonitoringConfigurationWindowModel { get; set; }

        public MonitoringConfigurationAddEditViewModel(IMessenger messenger,
            IConfigurationRepository configurationRepository, IAppLocalContextManager appLocalContextManager, IPowerpointParser powerpointParser, IFileDialogService fileDialogService, IBalloonService balloonService, ILoggerFactory loggerFactory)
        {
            _messenger = messenger;
            _configurationRepository = configurationRepository;
            _appLocalContextManager = appLocalContextManager;
            _powerpointParser = powerpointParser;
            _fileDialogService = fileDialogService;
            _balloonService = balloonService;
            _logger = loggerFactory.GetLogger(typeof(MonitoringConfigurationAddEditViewModel));
            SetupMessageRegistrations();
        }

        public ESettingsSubViewModel ESettingsSubViewModel =>
            ESettingsSubViewModel.MonitoringConfigurationAddEditViewModel;

        public RelayCommand<ScreenConfigurationWindowModel> DeleteCommand => new RelayCommand<ScreenConfigurationWindowModel>(
            screenConfiguration =>
            {
                ScreenConfigurations.Remove(screenConfiguration);
                var screenConfigToDelete =
                    _monitoringConfiguration.ScreenConfigurations.FirstOrDefault(o =>
                        o.Id == screenConfiguration.Id);

                if (screenConfigToDelete == null) return;

                _monitoringConfiguration.ScreenConfigurations.Remove(screenConfigToDelete);
                _configurationRepository.Save(_monitoringConfiguration);
            });

        public RelayCommand<ScreenConfigurationWindowModel> EditCommand => new RelayCommand<ScreenConfigurationWindowModel>(
                screenConfiguration =>
                {
                    _appLocalContextManager.SetScreenConfigurationId(screenConfiguration.Id);
                    _messenger.Send(new ShowEditScreenConfigurationMessage());
                    _messenger.Send(new HideSettingsMessage());
                });

        public RelayCommand SaveMonitoringConfigurationCommand => new RelayCommand(() =>
        {
            if (MonitoringConfigurationWindowModel.HasErrors)
            {
                return;
            }

            if (string.IsNullOrEmpty(MonitoringConfigurationWindowModel.Name))
            {
                MonitoringConfigurationWindowModel.Name = null;
                return;
            }

            _monitoringConfiguration.Name = MonitoringConfigurationWindowModel.Name;
            _monitoringConfiguration.ScreenConfigurations.ForEach(sc =>
            {
                sc.Number = ScreenConfigurations.First(o => o.Id == sc.Id).Number;
            });
            _configurationRepository.Save(_monitoringConfiguration);
            _appLocalContextManager.SetMonitoringConfigurationId(_monitoringConfiguration.Id);
            IsMonitoringConfigurationSaved = true;
        });

        public RelayCommand BackCommand => new RelayCommand(() =>
        {
            _messenger.Send(new ShowMonitoringConfigurationsMessage());
        });

        public AwaitableDelegateCommand CreateFromPptCommand => new AwaitableDelegateCommand(async () =>
        {
            try
            {
                var fileName = _fileDialogService.OpenFileDialog();

                // Cancelled window
                if (fileName == null)
                {
                    return;
                }

                _messenger.SendIsBusyChanged(true);

                await Task.Run(() =>
                {
                    var screenConfigurations = _powerpointParser.Parse(fileName).ToList();
                    _monitoringConfiguration.ScreenConfigurations = screenConfigurations;
                    ScreenConfigurations = new ObservableCollection<ScreenConfigurationWindowModel>(Convert(screenConfigurations));
                    _monitoringConfiguration.ScreenConfigurations.ForEach(sc =>
                    {
                        sc.Number = ScreenConfigurations.First(o => o.Id == sc.Id).Number;
                    });
                    _configurationRepository.Save(_monitoringConfiguration);
                    _appLocalContextManager.SetMonitoringConfigurationId(_monitoringConfiguration.Id);
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"Could not parse ppt file. Ex: {ex}");
                _balloonService.ShowBalloonTip("Error", "Could parse invalid ppt file.", BalloonIcon.Error);
            }
            finally
            {
                _messenger.SendIsBusyChanged(false);
            }
        });

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Monitoring configuration must be saved to db before actions on grid can be done
        /// </summary>
        public bool IsMonitoringConfigurationSaved
        {
            get => _isMonitoringConfigurationSaved;
            set => SetProperty(ref _isMonitoringConfigurationSaved, value);
        }

        public ScreenConfigurationWindowModel SelectedScreenConfiguration
        {
            get => _selectedScreenConfiguration;
            set
            {
                _selectedScreenConfiguration = value;
                if (_selectedScreenConfiguration != null)
                {
                    AddEditScreenConfigurationWindowModel = new ScreenConfigurationWindowModel
                    {
                        Name = _selectedScreenConfiguration.Name,
                        Duration = _selectedScreenConfiguration.Duration
                    };
                }
                OnPropertyChanged();
                AddEditScreenModeEnabled = true;
            }
        }

        public ScreenConfigurationWindowModel AddEditScreenConfigurationWindowModel
        {
            get => _addEditScreenConfigurationWindowModel;
            set
            {
                _addEditScreenConfigurationWindowModel = value;
                OnPropertyChanged();
            }
        }

        public bool AddEditScreenModeEnabled
        {
            get => _addEditScreenModeEnabled;
            set
            {
                _addEditScreenModeEnabled = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddScreenConfigurationCommand => new RelayCommand(() =>
        {
            AddEditScreenConfigurationWindowModel = new ScreenConfigurationWindowModel
            {
                Id = Guid.NewGuid().ToString()
            };
            AddEditScreenModeEnabled = true;
            SelectedScreenConfiguration = null;
        });

        public RelayCommand SaveScreenConfigurationCommand => new RelayCommand(() =>
        {
            if (!IsValidScreenConfiguration()) return;

            if (SelectedScreenConfiguration != null)
            {
                var windowScreenConfiguration = ScreenConfigurations.First(o => o.Id == SelectedScreenConfiguration.Id);
                windowScreenConfiguration.Name = AddEditScreenConfigurationWindowModel.Name;
                windowScreenConfiguration.Duration = AddEditScreenConfigurationWindowModel.Duration;

                var dbScreenConfiguration = _monitoringConfiguration.ScreenConfigurations.First(o => o.Id == SelectedScreenConfiguration.Id);
                dbScreenConfiguration.Name = AddEditScreenConfigurationWindowModel.Name;
                dbScreenConfiguration.Duration = ParseDuration(AddEditScreenConfigurationWindowModel.Duration.Value);
            }
            else
            {
                ScreenConfigurations.Add(AddEditScreenConfigurationWindowModel);
                _monitoringConfiguration.ScreenConfigurations.Add(new ScreenConfiguration
                {
                    Id = AddEditScreenConfigurationWindowModel.Id,
                    Name = AddEditScreenConfigurationWindowModel.Name,
                    Duration = ParseDuration(AddEditScreenConfigurationWindowModel.Duration.Value),
                    Number = ScreenConfigurations.Count - 1,
                    // Only set when added using hot key, otherwise null. When editing, it's handled in other screen.
                    AreasOfInterest = _addedAreasOfInterestWithUsingHotkey
                });

                _addedAreasOfInterestWithUsingHotkey = null;
            }

            _monitoringConfiguration.ScreenConfigurations.ForEach(sc =>
            {
                sc.Number = ScreenConfigurations.First(o => o.Id == sc.Id).Number;
            });
            _configurationRepository.Save(_monitoringConfiguration);
            _appLocalContextManager.SetScreenConfigurationId(AddEditScreenConfigurationWindowModel.Id);
            _appLocalContextManager.SetMonitoringConfigurationId(_monitoringConfiguration.Id);
        });

        private bool IsValidScreenConfiguration()
        {
            var isValid = !AddEditScreenConfigurationWindowModel.HasErrors;

            if (string.IsNullOrEmpty(AddEditScreenConfigurationWindowModel.Name))
            {
                isValid = false;
                AddEditScreenConfigurationWindowModel.Name = null;
            }
            if (!AddEditScreenConfigurationWindowModel.Duration.HasValue)
            {
                isValid = false;
                AddEditScreenConfigurationWindowModel.Duration = null;
            }

            return isValid;
        }

        private static List<ScreenConfigurationWindowModel> Convert(List<ScreenConfiguration> screenConfigurations)
        {
            var screenConfigurationWindowModels = new List<ScreenConfigurationWindowModel>();

            screenConfigurations?.ForEach(screenConfiguration =>
            {
                var screenConfigurationWindowModel = new ScreenConfigurationWindowModel
                {
                    AreasOfInterestCount = screenConfiguration.AreasOfInterest?.Count ?? 0,
                    Id = screenConfiguration.Id,
                    Name = screenConfiguration.Name,
                    Number = screenConfiguration.Number
                };

                if (screenConfiguration.Duration.HasValue)
                {
                    screenConfigurationWindowModel.Duration = ParseDuration(screenConfiguration.Duration.Value);
                }

                screenConfigurationWindowModels.Add(screenConfigurationWindowModel);
            });

            return screenConfigurationWindowModels;
        }

        private static DateTime ParseDuration(TimeSpan timespan)
        {
            return new DateTime(2019, 1, 1, timespan.Hours, timespan.Minutes, timespan.Seconds, DateTimeKind.Utc);
        }

        private static TimeSpan ParseDuration(DateTime dateTime)
        {
            return new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as ScreenConfigurationWindowModel;
            var targetItem = dropInfo.TargetItem as ScreenConfigurationWindowModel;

            if (sourceItem != null && targetItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as ScreenConfigurationWindowModel;
            var targetItem = dropInfo.TargetItem as ScreenConfigurationWindowModel;

            var removedIdx = ScreenConfigurations.IndexOf(sourceItem);
            var targetIdx = ScreenConfigurations.IndexOf(targetItem);

            if (removedIdx < targetIdx)
            {
                ScreenConfigurations.Insert(targetIdx + 1, sourceItem);
                ScreenConfigurations.RemoveAt(removedIdx);
            }
            else
            {
                var remIdx = removedIdx + 1;
                if (ScreenConfigurations.Count + 1 > remIdx)
                {
                    ScreenConfigurations.Insert(targetIdx, sourceItem);
                    ScreenConfigurations.RemoveAt(remIdx);
                }
            }
            
            for (var i = 0; i < ScreenConfigurations.Count; i++)
            {
                ScreenConfigurations[i].Number = i;
            }
        }

        private void SetupMessageRegistrations()
        {
            _messenger.Register<ShowEditMonitoringConfigurationMessage>(o =>
            {
                var screenConfigurationWindowModels = Convert(o.MonitoringConfiguration.ScreenConfigurations);
                ScreenConfigurations =
                    new ObservableCollection<ScreenConfigurationWindowModel>(screenConfigurationWindowModels.OrderBy(x => x.Number));
                _monitoringConfiguration = o.MonitoringConfiguration;
                MonitoringConfigurationWindowModel = new MonitoringConfigurationWindowModel
                {
                    Name = o.MonitoringConfiguration.Name,
                    ScreenConfigurations = screenConfigurationWindowModels
                };
                AddEditScreenConfigurationWindowModel = new ScreenConfigurationWindowModel();
                AddEditScreenModeEnabled = false;
                IsMonitoringConfigurationSaved = true;
            });

            _messenger.Register<ShowAddMonitoringConfigurationMessage>(o =>
            {
                ScreenConfigurations = new ObservableCollection<ScreenConfigurationWindowModel>();
                _monitoringConfiguration = new MonitoringConfiguration
                {
                    ScreenConfigurations = new List<ScreenConfiguration>()
                };
                MonitoringConfigurationWindowModel = new MonitoringConfigurationWindowModel
                {
                    ScreenConfigurations = new List<ScreenConfigurationWindowModel>()
                };
                AddEditScreenConfigurationWindowModel = new ScreenConfigurationWindowModel();
                AddEditScreenModeEnabled = false;
            });

            _messenger.Register<AddScreenConfigurationWithHotKeyMessage>(o =>
            {
                _addedAreasOfInterestWithUsingHotkey = o.ScreenConfiguration.AreasOfInterest;
                AddEditScreenConfigurationWindowModel = new ScreenConfigurationWindowModel
                {
                    Id = Guid.NewGuid().ToString()
                };
                AddEditScreenModeEnabled = true;
                SelectedScreenConfiguration = null;
                IsMonitoringConfigurationSaved = true;
            });
        }
    }
}