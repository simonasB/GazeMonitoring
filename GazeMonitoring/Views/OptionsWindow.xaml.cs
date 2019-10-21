using System.Windows;
using System.Windows.Controls;
using GazeMonitoring.Unmanaged;
using GazeMonitoring.ViewModels;

namespace GazeMonitoring.Views
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : UserControl
    {
        public OptionsWindow()
        {
            //this.DataContext = new OptionsViewModel(globalHotKeyManager);
            InitializeComponent();
        }
    }
}
