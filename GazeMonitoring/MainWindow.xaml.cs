using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.Model;
using GazeMonitoring.ScreenCapture;
using Constants = GazeMonitoring.Common.Constants;

namespace GazeMonitoring {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private GazeDataMonitor _gazeDataMonitor;
        private readonly IContainer _container;
        private static ILifetimeScope _lifetimeScope;
        private SubjectInfo _subjectInfo;
        private IScreenRecorder _screenRecorder;

        public MainWindow(IContainer container) {
            _container = container;
            InitializeComponent();

            CmbDataStreams.ItemsSource = Enum.GetValues(typeof(DataStream)).Cast<DataStream>();
        }

        private void CmbDataStreams_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) {
            _lifetimeScope = _container.BeginLifetimeScope();
            _subjectInfo = new SubjectInfo();

            int.TryParse(TextBoxAge.Text, out var age);

            if (CheckBoxAnonymous.IsChecked != true) {
                _subjectInfo = new SubjectInfo {
                    Name = TextBoxName.Text,
                    Age = age,
                    Details = TextBoxDetails.Text,
                };
            }

            _subjectInfo.SessionId = Guid.NewGuid().ToString();
            _subjectInfo.SessionStartTimestamp = DateTime.UtcNow;
            _gazeDataMonitor = _lifetimeScope.Resolve<GazeDataMonitor>(
                new NamedParameter(Constants.DataStreamParameterName, CmbDataStreams.SelectedItem),
                new NamedParameter(Constants.SubjectInfoParameterName, _subjectInfo));
            _gazeDataMonitor.Start();

            var recordScreen = true;

            if (recordScreen) {
                _screenRecorder = _lifetimeScope.Resolve<IScreenRecorder>(
                    new NamedParameter(Constants.DataStreamParameterName, CmbDataStreams.SelectedItem),
                    new NamedParameter(Constants.RecorderParamsParameterName, new RecorderParams($"video_{CmbDataStreams.SelectedItem}_{DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_fff", CultureInfo.InvariantCulture)}.avi", 10, 50)));
                _screenRecorder.StartRecording();
            }

            ToggleFieldsOnStartAndStop(false);
        }

        private async void BtnStop_Click(object sender, RoutedEventArgs e) {
            _gazeDataMonitor.Stop();
            _lifetimeScope.Dispose();
            _subjectInfo.SessionEndTimeStamp = DateTime.UtcNow;

            var selectedItem = CmbDataStreams.SelectedItem;

            await Task.Run(() => {
                using (var lifetimeScope = _container.BeginLifetimeScope()) {
                    var finalizer = lifetimeScope.Resolve<IGazeDataMonitorFinalizer>(
                        new NamedParameter(Constants.DataStreamParameterName, selectedItem),
                        new NamedParameter(Constants.SubjectInfoParameterName, _subjectInfo));
                    finalizer.FinalizeMonitoring();
                }

                _screenRecorder.StopRecording();
            });

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
