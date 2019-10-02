using System.Windows;
using GazeMonitoring.Unmanaged;
using GazeMonitoring.ViewModels;

namespace GazeMonitoring.Views
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow(IGlobalHotKeyManager globalHotKeyManager)
        {
            this.DataContext = new OptionsViewModel(globalHotKeyManager);
            InitializeComponent();
        }
    }
}
