using System;
using System.Linq;
using System.Windows;
using GazeMonitoring.Model;
using GazeMonitoring.ViewModels;

namespace GazeMonitoring.Views {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow(MainViewModel mainViewModel) {
            this.DataContext = mainViewModel;
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
