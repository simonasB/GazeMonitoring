using System;
using System.Windows;

namespace GazeMonitoring.Views
{
    /// <summary>
    /// Interaction logic for MainNavigationWindow.xaml
    /// </summary>
    public partial class MainNavigationWindow : Window
    {
        public MainNavigationWindow()
        {
            InitializeComponent();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }
}
