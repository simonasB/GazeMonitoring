using System.ComponentModel;
using System.Windows;
using GazeMonitoring.ViewModels;

namespace GazeMonitoring.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(SettingsViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
            //_messenger.Register<ShowSettingsMessage>(_ => this.Show());
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
