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
using Hardcodet.Wpf.TaskbarNotification;
using Constants = GazeMonitoring.Common.Constants;

namespace GazeMonitoring {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow(IContainer container, TaskbarIcon notifyIcon) {
            this.DataContext = new MainViewModel(container, notifyIcon);
            InitializeComponent();
            CmbDataStreams.ItemsSource = Enum.GetValues(typeof(DataStream)).Cast<DataStream>();
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
