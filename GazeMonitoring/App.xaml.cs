using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GazeMonitoring.Base;
using GazeMonitoring.DataAccess.LiteDB;
using GazeMonitoring.Discovery;
using GazeMonitoring.IoC;
using GazeMonitoring.Logging;
using GazeMonitoring.Model;
using GazeMonitoring.Monitor;
using GazeMonitoring.Powerpoint;
using GazeMonitoring.Unmanaged;
using GazeMonitoring.ViewModels;
using GazeMonitoring.Views;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.Configuration;

namespace GazeMonitoring
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _taskbarIcon;
        private static IoContainer _container;
        private ILogger _logger;

        private GlobalHotKey _hotKey;
        private IAppLocalContext _appLocalContext;

        private GlobalHotKey _parseGlobalHotKey;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _appLocalContext = new AppLocalContext();

            try
            {
                Init();
                _logger = _container.GetInstance<ILoggerFactory>().GetLogger(typeof(App));
                //_taskbarIcon.DataContext = new NotifyIconViewModel(new Views.MainWindow(_container, new BalloonService(_taskbarIcon)));
                _taskbarIcon = _container.GetInstance<TaskbarIcon>();
                _taskbarIcon.DataContext = _container.GetInstance<NotifyIconViewModel>();

                SetupExceptionHandling();
            } catch (Exception ex)
            {
                _logger?.Error(ex);
                MessageBox.Show($"Could not launch GazeMonitoring application.{ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }

            _hotKey = new GlobalHotKey(Key.F9, ModifierKeys.None, new EditScreenConfigurationHandler(_appLocalContext));
            _parseGlobalHotKey = new GlobalHotKey(Key.F10, ModifierKeys.None, () =>
            {
                var parser = new PowerpointParser(new DefaultScreenParameters());
                var configurations = parser.Parse().ToList();
                var repo = new LiteDBConfigurationRepository();
                var recordingConfiguration = new MonitoringConfiguration
                {
                    Name = "ppt",
                    ScreenConfigurations = configurations
                };
                var id = repo.Save(recordingConfiguration);
            });
        }

        private void SetupExceptionHandling() {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception) e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");

            TaskScheduler.UnobservedTaskException += (s, e) =>
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
        }

        private void LogUnhandledException(Exception exception, string source) {
            try {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                var message = $"Unhandled exception in {assemblyName.Name} v{assemblyName.Version}";
                _logger.Error(message);
            } catch (Exception ex) {
                _logger.Error(ex);
            } finally {
                _logger.Error(exception);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _taskbarIcon?.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }

        private void Init() {
            var config = new ConfigurationBuilder();

            config.AddJsonFile("config.json");
            var configurationRoot = config.Build();
            if (!bool.TryParse(configurationRoot["autoDiscover"], out var autoDiscover)) {
                _logger.Debug("AutoDiscovery set to false");
            }

            var builder = ContainerBuilderFactory.Create();

            builder.RegisterModule<CommonModule>();
            builder.RegisterModule(configurationRoot);
            builder.Register<IScreenParameters, DefaultScreenParameters>();
            builder.Register<IGazeDataMonitorFactory, GazeDataMonitorFactory>();

            if (autoDiscover) {
                var discoveryManager = new TrackerDiscoveryManager();
                discoveryManager.Discover(builder);
            }

            var notifyIcon = (TaskbarIcon) FindResource("NotifyIcon");

            if (notifyIcon == null)
            {
                // fail
            }

            builder.RegisterSingleton(notifyIcon);
            builder.Register<IBalloonService, BalloonService>();
            builder.Register<MainViewModel>();
            builder.Register<MainWindow>();
            builder.Register<NotifyIconViewModel>();

            _container = builder.Build();
        }
    }
}
