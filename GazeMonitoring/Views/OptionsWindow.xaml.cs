using System.Windows;
using GazeMonitoring.ViewModels;

namespace GazeMonitoring.Views
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            this.DataContext = new OptionsViewModel();
            InitializeComponent();
        }
    }
}
