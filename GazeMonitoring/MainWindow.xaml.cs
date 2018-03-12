using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;
using GazeMonitoring.Data.Csv;
using TheEyeTribeMonitoring;
using Tobii.Interaction;
using TobiiCoreMonitoring;
using TobiiCoreMonitoring.Csv;

namespace GazeMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private IDataStreamToExternalStorageWriter _externalStorageWriter;
        private readonly IDataStreamToExternalStorageWriterFactory _externalStorageWriterFactory;
        private readonly Host _host;
        private GazeDataMonitor _gazeDataMonitor;

        public MainWindow() {
            InitializeComponent();
            CmbDataStreams.ItemsSource = Enum.GetValues(typeof(DataStream)).Cast<DataStream>();
            _host = new Host();
            _externalStorageWriterFactory = new DataStreamToCsvWriterFactory(_host);
        }

        private void CmbDataStreams_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) {
            var dataStream = (DataStream) CmbDataStreams.SelectedItem;

            // Dispose csvWritersManager

            var repository = new CsvGazeDataRepository(new CsvWritersManager(new CsvWritersFactory(new FileNameFormatter()), dataStream));
            var streamFactory = new EyeTribeGazePointStreamFactory();
            var gazePointStream = streamFactory.GetGazePointStream(dataStream);

            var gazeDataWriterFactory = new GazeDataWriterFactory(repository, new BasicSaccadeCalculator());

            _gazeDataMonitor = new GazeDataMonitor(gazePointStream, gazeDataWriterFactory.GetGazeDataWriter(dataStream));

            _gazeDataMonitor.Start();

            _externalStorageWriter = _externalStorageWriterFactory.GetWriter((DataStream)CmbDataStreams.SelectedItem);
            _externalStorageWriter?.Write();
            BtnStart.IsEnabled = false;
            CmbDataStreams.IsEnabled = false;
            BtnStop.IsEnabled = true;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e) {
            _gazeDataMonitor.Start();

            _externalStorageWriter?.Dispose();
            BtnStart.IsEnabled = true;
            CmbDataStreams.IsEnabled = true;
            BtnStop.IsEnabled = false;
        }
    }
}
