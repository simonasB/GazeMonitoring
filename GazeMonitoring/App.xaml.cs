using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Autofac;
using Autofac.Configuration;
using Autofac.Core;
using GazeMonitoring.Base;
using GazeMonitoring.Common;
using GazeMonitoring.Data.Writers;
using GazeMonitoring.Discovery;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.IoC;
using GazeMonitoring.Logging;
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
        private static IContainer _container;
        private ILogger _logger;

        private GlobalHotKey _hotKey;
        private IAppLocalContext _appLocalContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _appLocalContext = new AppLocalContext();

            try
            {
                Init();
                //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
                _taskbarIcon = (TaskbarIcon) FindResource("NotifyIcon");

                _logger = _container.Resolve<ILoggerFactory>().GetLogger(typeof(App));
                _taskbarIcon.DataContext = new NotifyIconViewModel(new Views.MainWindow(_container, new BalloonService(_taskbarIcon)));


                //var screenConfigurationWindow = new ScreenConfigurationWindow();
                //screenConfigurationWindow.Show();

                SetupExceptionHandling();
            } catch (Exception ex)
            {
                _logger?.Error(ex);
                MessageBox.Show($"Could not launch GazeMonitoring application.{ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }

            _hotKey = new GlobalHotKey(Key.F9, KeyModifier.None, new EditScreenConfigurationHandler(_appLocalContext));
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
            string message = $"Unhandled exception ({source})";
            try {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
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

            var builder = new ContainerBuilder();
            var module = new ConfigurationModule(configurationRoot);
            builder.RegisterModule<CommonModule>();
            builder.RegisterModule(module);
            builder.Register((c, p) => {
                var parameters = p as Parameter[] ?? p.ToArray();
                return new GazeDataMonitor(c.Resolve<GazePointStream>(parameters), c.Resolve<IGazeDataWriter>(parameters));
            }).As<IGazeDataMonitor>();
            builder.RegisterType<DefaultScreenParameters>().As<IScreenParameters>();

            if (autoDiscover) {
                var discoveryManager = new TrackerDiscoveryManager();
                discoveryManager.Discover(builder);
            }
            _container = builder.Build();
        }
    }
}
