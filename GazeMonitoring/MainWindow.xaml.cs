using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using Autofac.Configuration;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;
using Microsoft.Extensions.Configuration;
using Constants = GazeMonitoring.Common.Constants;

namespace GazeMonitoring {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private GazeDataMonitor _gazeDataMonitor;
        private static IContainer _container;
        private static ILifetimeScope _lifetimeScope;
        private SubjectInfo _subjectInfo;

        public MainWindow() {
            InitializeComponent();

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
            _subjectInfo = new SubjectInfo();

            if (CheckBoxAnonymous.IsChecked != true) {
                _subjectInfo = new SubjectInfo {
                    Name = TextBoxName.Text,
                    Age = int.Parse(TextBoxAge.Text),
                    Details = TextBoxDetails.Text,
                };
            }

            _subjectInfo.SessionId = Guid.NewGuid().ToString();

            _gazeDataMonitor = _lifetimeScope.Resolve<GazeDataMonitor>(
                new NamedParameter(Constants.DataStreamParameterName, CmbDataStreams.SelectedItem),
                new NamedParameter(Constants.SubjectInfoParameterName, _subjectInfo));

            _gazeDataMonitor.Start();

            ToggleFieldsOnStartAndStop(false);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e) {
            _gazeDataMonitor.Stop();
            _lifetimeScope.Dispose();

            using (var lifetimeScope = _container.BeginLifetimeScope()) {
                var finalizer = lifetimeScope.Resolve<IGazeDataMonitorFinalizer>(
                    new NamedParameter(Constants.DataStreamParameterName, CmbDataStreams.SelectedItem),
                    new NamedParameter(Constants.SubjectInfoParameterName, _subjectInfo));
                finalizer.FinalizeMonitoring();
            }

            ToggleFieldsOnStartAndStop(true);
        }

        private void CheckBoxAnonymousChanged(object sender, RoutedEventArgs e) {
            void ToggleFields(bool isEnabled) {
                TextBoxAge.IsEnabled = isEnabled;
                TextBoxDetails.IsEnabled = isEnabled;
                TextBoxName.IsEnabled = isEnabled;
            }

            ToggleFields(CheckBoxAnonymous.IsChecked != true);
        }

        private void ToggleFieldsOnStartAndStop(bool isEnabled) {
            BtnStart.IsEnabled = isEnabled;
            CmbDataStreams.IsEnabled = isEnabled;
            CheckBoxAnonymous.IsEnabled = isEnabled;
            BtnStop.IsEnabled = !isEnabled;
        }

        private void Window_Deactivated(object sender, EventArgs e) {
            Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }
}
