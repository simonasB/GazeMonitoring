using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Tobii.Interaction;
using TobiiCoreMonitoring;
using TobiiCoreMonitoring.csv;
using TobiiCoreMonitoring.Entities;

namespace GazeMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private IDataStreamToExternalStorageWriter _externalStorageWriter;
        private readonly IDataStreamToExternalStorageWriterFactory _externalStorageWriterFactory;
        private readonly Host _host;

        public MainWindow() {
            InitializeComponent();
            CmbDataStreams.ItemsSource = Enum.GetValues(typeof(DataStream)).Cast<DataStream>();
            _host = new Host();
            _externalStorageWriterFactory = new DataStreamToCsvWriterFactory(_host);
        }

        private void CmbDataStreams_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) {
            _externalStorageWriter = _externalStorageWriterFactory.GetWriter((DataStream)CmbDataStreams.SelectedItem);
            _externalStorageWriter?.Write();
            BtnStart.IsEnabled = false;
            CmbDataStreams.IsEnabled = false;
            BtnStop.IsEnabled = true;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e) {
            _externalStorageWriter?.Dispose();
            BtnStart.IsEnabled = true;
            CmbDataStreams.IsEnabled = true;
            BtnStop.IsEnabled = false;
        }
    }
}
