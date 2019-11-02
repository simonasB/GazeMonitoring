using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                var screenConfigurationWindowModels = new List<ScreenConfigurationWindowModel>();
                o.MonitoringConfiguration.ScreenConfigurations?.ForEach(screenConfiguration =>
                {
                    screenConfigurationWindowModels.Add(new ScreenConfigurationWindowModel
                    {
                        AreasOfInterestCount = screenConfiguration.AreasOfInterest?.Count ?? 0,
                        Id = screenConfiguration.Id,
                        Name = screenConfiguration.Name
                    });
                });
                ScreenConfigurations =
                    new ObservableCollection<ScreenConfigurationWindowModel>(screenConfigurationWindowModels);
                _monitoringConfiguration = o.MonitoringConfiguration;
                MonitoringConfigurationWindowModel = new MonitoringConfigurationWindowModel
                {
                    Name = o.MonitoringConfiguration.Name,
                    ScreenConfigurations = screenConfigurationWindowModels
                };
            });

            _messenger.Register<ShowAddMonitoringConfigurationMessage>(o =>
            {
                ScreenConfigurations = new ObservableCollection<ScreenConfigurationWindowModel>();
                _monitoringConfiguration = new MonitoringConfiguration();
                MonitoringConfigurationWindowModel = new MonitoringConfigurationWindowModel
                {
                    ScreenConfigurations = new List<ScreenConfigurationWindowModel>()
                };
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
            });

        public RelayCommand BackCommand => new RelayCommand(() =>
        {
            _messenger.Send(new ShowMonitoringConfigurationsMessage());
        });

        public RelayCommand CreateFromPptCommand => new RelayCommand(() =>
        {
            var fileName = _fileDialogService.OpenFileDialog();

            // Cancelled window
            if (fileName == null)
            {
                return;
            }

            var screenConfigurations = _powerpointParser.Parse(fileName).ToList();
            _monitoringConfiguration.ScreenConfigurations = screenConfigurations;
            ScreenConfigurations = new ObservableCollection<ScreenConfigurationWindowModel>(Convert(screenConfigurations));
            _configurationRepository.Save(_monitoringConfiguration);
        });

        private List<ScreenConfigurationWindowModel> Convert(List<ScreenConfiguration> screenConfigurations)
        {
            var screenConfigurationWindowModels = new List<ScreenConfigurationWindowModel>();

            screenConfigurations?.ForEach(screenConfiguration =>
            {
                screenConfigurationWindowModels.Add(new ScreenConfigurationWindowModel
                {
                    AreasOfInterestCount = screenConfiguration.AreasOfInterest?.Count ?? 0,
                    Id = screenConfiguration.Id,
                    Name = screenConfiguration.Name
                });
            });

            return screenConfigurationWindowModels;
        }
    }
}