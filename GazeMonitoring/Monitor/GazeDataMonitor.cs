using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.Data.Writers;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;
using GazeMonitoring.ScreenCapture;

namespace GazeMonitoring.Monitor {
    public class GazeDataMonitor : IGazeDataMonitor {
        private readonly GazePointStream _gazePointStream;
        private readonly IGazeDataWriter _gazeDataWriter;
        private readonly IScreenRecorder _screenRecorder;
        private readonly IGazeDataMonitorFinalizer _gazeDataMonitorFinalizer;
        private readonly IDataAggregationService _dataAggregationService;
        private readonly IMonitoringContext _monitoringContext;

        public GazeDataMonitor(GazePointStream gazePointStream, IGazeDataWriter gazeDataWriter, IMonitoringContext monitoringContext, IScreenRecorder screenRecorder, IGazeDataMonitorFinalizer gazeDataMonitorFinalizer, IDataAggregationService dataAggregationService) {
            if (gazePointStream == null) {
                throw new ArgumentNullException(nameof(gazePointStream));
            }
            if (gazeDataWriter == null) {
                throw new ArgumentNullException(nameof(gazeDataWriter));
            }
            _gazePointStream = gazePointStream;
            _gazeDataWriter = gazeDataWriter;
            _monitoringContext = monitoringContext;
            _screenRecorder = screenRecorder;
            _gazeDataMonitorFinalizer = gazeDataMonitorFinalizer;
            _dataAggregationService = dataAggregationService;
        }

        public Task StartAsync() {
            _gazePointStream.GazePointReceived += OnGazePointReceived;
            if (_monitoringContext.IsScreenRecorded)
            {
                _screenRecorder.StartRecording(
                    new RecorderParams(
                        Path.Combine(_monitoringContext.DataFilesPath, $"video_{_monitoringContext.DataStream}_{_monitoringContext.SubjectInfo.SessionStartTimestamp.ToString("yyyy_MM_dd_HH_mm_ss_fff", CultureInfo.InvariantCulture)}.avi"),
                        10, 50), _monitoringContext);
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync() {
            _gazePointStream.GazePointReceived -= OnGazePointReceived;
            _gazeDataWriter.Dispose();

            var finalizationTask = Task.Run(() =>
            {
                _gazeDataMonitorFinalizer.FinalizeMonitoring(_monitoringContext);
            });

            var stopRecordingTask = Task.Run(() => {
                if (_monitoringContext.IsScreenRecorded)
                {
                    _screenRecorder?.StopRecording();
                }
            });

            await Task.WhenAll(finalizationTask, stopRecordingTask).ConfigureAwait(false);
            if (_monitoringContext.MonitoringConfiguration != null)
            {
                await _dataAggregationService.Run(_monitoringContext).ConfigureAwait(false);
            }
        }

        private void OnGazePointReceived(object sender, GazePointReceivedEventArgs args)
        {
            _gazeDataWriter.Write(args.GazePoint);
        }
    }
}
