using GazeMonitoring.Data.Writers;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;

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

        public GazeDataMonitorFactory(IGazePointStreamFactory gazePointStreamFactory, IGazeDataWriterFactory gazeDataWriterFactory)
        {
            _gazePointStreamFactory = gazePointStreamFactory;
            _gazeDataWriterFactory = gazeDataWriterFactory;
        }

        public IGazeDataMonitor Create(IMonitoringContext context)
        {
            return new GazeDataMonitor(_gazePointStreamFactory.GetGazePointStream(context.DataStream), _gazeDataWriterFactory.Create(context));
        }
    }
}
