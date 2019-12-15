using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.Data.Writers;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;
using GazeMonitoring.ScreenCapture;

namespace GazeMonitoring.Monitor
{
    public interface IGazeDataMonitorFactory
    {
        IGazeDataMonitor Create(IMonitoringContext context);
    }

    public class GazeDataMonitorFactory : IGazeDataMonitorFactory
    {
        private readonly IGazePointStreamFactory _gazePointStreamFactory;
        private readonly IGazeDataWriterFactory _gazeDataWriterFactory;
        private readonly IScreenRecorder _screenRecorder;
        private readonly IGazeDataMonitorFinalizer _gazeDataMonitorFinalizer;
        private readonly IDataAggregationService _dataAggregationService;

        public GazeDataMonitorFactory(IGazePointStreamFactory gazePointStreamFactory, IGazeDataWriterFactory gazeDataWriterFactory, IScreenRecorder screenRecorder, IGazeDataMonitorFinalizer gazeDataMonitorFinalizer, IDataAggregationService dataAggregationService)
        {
            _gazePointStreamFactory = gazePointStreamFactory;
            _gazeDataWriterFactory = gazeDataWriterFactory;
            _screenRecorder = screenRecorder;
            _gazeDataMonitorFinalizer = gazeDataMonitorFinalizer;
            _dataAggregationService = dataAggregationService;
        }

        public IGazeDataMonitor Create(IMonitoringContext context)
        {
            return new GazeDataMonitor(_gazePointStreamFactory.GetGazePointStream(context.DataStream), _gazeDataWriterFactory.Create(context), context, _screenRecorder, _gazeDataMonitorFinalizer, _dataAggregationService);
        }
    }
}
