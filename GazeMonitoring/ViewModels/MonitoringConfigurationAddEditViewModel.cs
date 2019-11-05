using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GazeMonitoring.Base;
using GazeMonitoring.Commands;
using GazeMonitoring.DataAccess;
using GazeMonitoring.IO;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using GazeMonitoring.Model;
using GazeMonitoring.Powerpoint;
using GazeMonitoring.WindowModels;

namespace GazeMonitoring.ViewModels
{
    public class MonitoringConfigurationAddEditViewModel : ViewModelBase, ISettingsSubViewModel
    {
        private readonly IMessenger _messenger;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IAppLocalContextManager _appLocalContextManager;
        private readonly IPowerpointParser _powerpointParser;
        private readonly IFileDialogService _fileDialogService;

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

        public MonitoringConfigurationWindowModel MonitoringConfigurationWindowModel { get; set; }

        public MonitoringConfigurationAddEditViewModel(IMessenger messenger,
            IConfigurationRepository configurationRepository, IAppLocalContextManager appLocalContextManager, IPowerpointParser powerpointParser, IFileDialogService fileDialogService)
        {
            _messenger = messenger;
            _configurationRepository = configurationRepository;
            _appLocalContextManager = appLocalContextManager;
            _powerpointParser = powerpointParser;
            _fileDialogService = fileDialogService;

            _messenger.Register<ShowEditMonitoringConfigurationMessage>(o =>
            {
                var screenConfigurationWindowModels = Convert(o.MonitoringConfiguration.ScreenConfigurations);
                ScreenConfigurations =
                    new ObservableCollection<ScreenConfigurationWindowModel>(screenConfigurationWindowModels);
                _monitoringConfiguration = o.MonitoringConfiguration;
                MonitoringConfigurationWindowModel = new MonitoringConfigurationWindowModel
                {
                    Name = o.MonitoringConfiguration.Name,
                    ScreenConfigurations = screenConfigurationWindowModels
                };
                AddEditScreenConfigurationWindowModel = new ScreenConfigurationWindowModel();
                AddEditScreenModeEnabled = false;
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
                _configurationRepository.Update(_monitoringConfiguration);
            });

        public RelayCommand<ScreenConfigurationWindowModel> EditCommand => new RelayCommand<ScreenConfigurationWindowModel>(
                screenConfiguration =>
                {
                    _appLocalContextManager.SetScreenConfigurationId(screenConfiguration.Id);
                    _messenger.Send(new ShowEditScreenConfigurationMessage());
                });

        public RelayCommand SaveMonitoringConfigurationCommand => new RelayCommand(() =>
            {
                _monitoringConfiguration.Name = MonitoringConfigurationWindowModel.Name;
                _configurationRepository.Save(_monitoringConfiguration);
                _appLocalContextManager.SetMonitoringConfigurationId(_monitoringConfiguration.Id);
            });

        public RelayCommand BackCommand => new RelayCommand(() =>
        {
            _messenger.Send(new ShowMonitoringConfigurationsMessage());
        });

        public AwaitableDelegateCommand CreateFromPptCommand => new AwaitableDelegateCommand(async () =>
        {
            var fileName = _fileDialogService.OpenFileDialog();

            // Cancelled window
            if (fileName == null)
            {
                return;
            }

            // TODO: might warp to try/finally
            IsBusy = true;

            await Task.Run(() =>
            {
                var screenConfigurations = _powerpointParser.Parse(fileName).ToList();
                _monitoringConfiguration.ScreenConfigurations = screenConfigurations;
                ScreenConfigurations = new ObservableCollection<ScreenConfigurationWindowModel>(Convert(screenConfigurations));
                _configurationRepository.Save(_monitoringConfiguration);
            });

            IsBusy = false;
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
            if (SelectedScreenConfiguration != null)
            {
                var windowScreenConfiguration = ScreenConfigurations.First(o => o.Id == SelectedScreenConfiguration.Id);
                windowScreenConfiguration.Name = AddEditScreenConfigurationWindowModel.Name;
                windowScreenConfiguration.Duration = AddEditScreenConfigurationWindowModel.Duration;

                var dbScreenConfiguration = _monitoringConfiguration.ScreenConfigurations.First(o => o.Id == SelectedScreenConfiguration.Id);
                dbScreenConfiguration.Name = AddEditScreenConfigurationWindowModel.Name;
                dbScreenConfiguration.Duration = ParseDuration(AddEditScreenConfigurationWindowModel.Duration);
                _configurationRepository.Save(_monitoringConfiguration);
                _appLocalContextManager.SetScreenConfigurationId(SelectedScreenConfiguration.Id);
            }
            else
            {
                ScreenConfigurations.Add(AddEditScreenConfigurationWindowModel);
                _monitoringConfiguration.ScreenConfigurations.Add(new ScreenConfiguration
                {
                    Id = AddEditScreenConfigurationWindowModel.Id,
                    Name = AddEditScreenConfigurationWindowModel.Name,
                    Duration = ParseDuration(AddEditScreenConfigurationWindowModel.Duration)
                });
                _configurationRepository.Save(_monitoringConfiguration);
                _appLocalContextManager.SetScreenConfigurationId(AddEditScreenConfigurationWindowModel.Id);
            }
            _appLocalContextManager.SetMonitoringConfigurationId(_monitoringConfiguration.Id);
        });

        private static List<ScreenConfigurationWindowModel> Convert(List<ScreenConfiguration> screenConfigurations)
        {
            var screenConfigurationWindowModels = new List<ScreenConfigurationWindowModel>();

            screenConfigurations?.ForEach(screenConfiguration =>
            {
                var screenConfigurationWindowModel = new ScreenConfigurationWindowModel
                {
                    AreasOfInterestCount = screenConfiguration.AreasOfInterest?.Count ?? 0,
                    Id = screenConfiguration.Id,
                    Name = screenConfiguration.Name
                };

                if (screenConfiguration.Duration.HasValue)
                {
                    screenConfigurationWindowModel.Duration = new DateTime(2019,1,1,screenConfiguration.Duration.Value.Minutes, screenConfiguration.Duration.Value.Seconds, 0, DateTimeKind.Utc);
                }

                screenConfigurationWindowModels.Add(screenConfigurationWindowModel);
            });

            return screenConfigurationWindowModels;
        }

        private static TimeSpan ParseDuration(DateTime dateTime)
        {
            // hours, minutes, seconds
            // Hours used for minutes because time picker does not support picking only minutes and seconds.
            return new TimeSpan(0, dateTime.Hour, dateTime.Minute);
        }
    }
}