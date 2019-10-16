using GazeMonitoring.Common.Calculations;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.Data;
using GazeMonitoring.Data.Writers;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Logging;
using GazeMonitoring.Logging.Log4Net;

namespace GazeMonitoring.IoC
{
    public class CommonModule : IoCModule
    {
        public void Load(IoContainerBuilder builder)
        {
            builder.Register<ISaccadeCalculator, BasicSaccadeCalculator>();
            builder.Register<IGazeDataMonitorFinalizer, NullGazeDataMonitorFinalizer>();
            builder.Register<IEyeTrackerStatusProvider, NullEyeTrackerStatusProvider>();
            builder.Register<ILoggerFactory, XmlLog4NetLoggerFactory>();
            builder.Register<IGazeDataRepositoryFactory, NullGazeDataRepositoryFactory>();
            builder.Register<IGazePointStreamFactory, NullGazePointStreamFactory>();
            builder.Register<IGazeDataWriterFactory, GazeDataWriterFactory>();
        }
    }
}
