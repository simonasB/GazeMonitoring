using Autofac;
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
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BasicSaccadeCalculator>().As<ISaccadeCalculator>();
            builder.RegisterType<NullGazeDataMonitorFinalizer>().As<IGazeDataMonitorFinalizer>();
            builder.RegisterType<NullEyeTrackerStatusProvider>().As<IEyeTrackerStatusProvider>();
            builder.RegisterType<XmlLog4NetLoggerFactory>().As<ILoggerFactory>();
            builder.RegisterType<NullGazeDataRepositoryFactory>().As<IGazeDataRepositoryFactory>();
            builder.RegisterType<NullGazePointStreamFactory>().As<IGazePointStreamFactory>();
            builder.RegisterType<GazeDataWriterFactory>().As<IGazeDataWriterFactory>();
        }
    }
}
