﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Autofac.Configuration;
using Autofac.Core;
using GazeMonitoring.Data.Writers;
using GazeMonitoring.EyeTracker.Core.Discovery;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.IoC;
using GazeMonitoring.Logging;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.Configuration;

namespace GazeMonitoring
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _notifyIcon;
        private static IContainer _container;
        private ILogger _logger;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Init();
            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            
            _notifyIcon.DataContext = new NotifyIconViewModel(new MainWindow(_container, _notifyIcon));

            _logger = _container.Resolve<ILoggerFactory>().GetLogger(typeof(App));

            SetupExceptionHandling();
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
            } catch (Exception ex) {
                _logger.Error(ex);
            } finally {
                _logger.Error(exception);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }

        private static void Init() {
            var config = new ConfigurationBuilder();

            config.AddJsonFile("config.json");
            var configurationRoot = config.Build();
            if (!bool.TryParse(configurationRoot["autoDiscover"], out var autoDiscover)) {
                // log info message
            }

            var builder = new ContainerBuilder();
            var module = new ConfigurationModule(configurationRoot);
            builder.RegisterModule<CommonModule>();
            builder.RegisterModule(module);
            builder.Register((c, p) => {
                var parameters = p as Parameter[] ?? p.ToArray();
                return new GazeDataMonitor(c.Resolve<GazePointStream>(parameters), c.Resolve<IGazeDataWriter>(parameters));
            }).As<GazeDataMonitor>();

            if (autoDiscover) {
                var discoveryManager = new TrackerDiscoveryManager();
                discoveryManager.Discover(builder);
            }

            _container = builder.Build();
        }
    }
}
