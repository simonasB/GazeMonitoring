﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using Autofac.Configuration;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;
using Microsoft.Extensions.Configuration;
using MockMonitoring;
using TheEyeTribeMonitoring;
using TobiiCoreMonitoring;

namespace GazeMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private GazeDataMonitor _gazeDataMonitor;
        private static IContainer _container;
        private static ILifetimeScope _lifetimeScope;

        public MainWindow() {
            InitializeComponent();
            EyeTribeInitializer.Init();
            TobiiCoreInitializer.Init();
            MockInitializer.Init();

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

            CmbDataStreams.ItemsSource = Enum.GetValues(typeof(DataStream)).Cast<DataStream>();

            _container = builder.Build();
        }

        private void CmbDataStreams_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) {
            _lifetimeScope = _container.BeginLifetimeScope();

            var subjectInfo = new SubjectInfo {
                Age = 10,
                Details = "testing",
                Name = "default",
                DataStream = (DataStream) CmbDataStreams.SelectedItem
            };

            _gazeDataMonitor = _lifetimeScope.Resolve<GazeDataMonitor>(
                new NamedParameter(Constants.DataStreamParameterName, CmbDataStreams.SelectedItem),
                new NamedParameter(Constants.SubjectInfoParameterName, subjectInfo));

            _gazeDataMonitor.Start();

            BtnStart.IsEnabled = false;
            CmbDataStreams.IsEnabled = false;
            BtnStop.IsEnabled = true;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e) {
            _gazeDataMonitor.Stop();
            _lifetimeScope.Dispose();

            BtnStart.IsEnabled = true;
            CmbDataStreams.IsEnabled = true;
            BtnStop.IsEnabled = false;
        }
    }
}
