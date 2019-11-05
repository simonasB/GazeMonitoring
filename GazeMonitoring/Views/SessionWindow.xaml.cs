using System;
using System.Linq;
using System.Windows.Controls;
using GazeMonitoring.Model;

namespace GazeMonitoring.Views {
    /// <summary>
    /// Interaction logic for SessionWindow.xaml
    /// </summary>
    public partial class SessionWindow : UserControl {
        public SessionWindow() {
            InitializeComponent();
            CmbDataStreams.ItemsSource = Enum.GetValues(typeof(DataStream)).Cast<DataStream>();
        }
    }
}
