﻿using System.Windows;
using Autofac;
using Autofac.Configuration;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Discovery;
using GazeMonitoring.Common.IoC;
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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Init();
            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            
            _notifyIcon.DataContext = new NotifyIconViewModel(new MainWindow(_container));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }

        private void Init() {
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

            if (autoDiscover) {
                var discoveryManager = new TrackerDiscoveryManager();
                discoveryManager.Discover(builder);
            }

            _container = builder.Build();
        }
    }
}
