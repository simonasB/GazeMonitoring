using System.Linq;
using Autofac;
using Autofac.Core;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Calculations;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.Data;
using GazeMonitoring.Data.Writers;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Logging;
using GazeMonitoring.Logging.Log4Net;
using GazeMonitoring.Model;

namespace GazeMonitoring.IoC {
    public class CommonModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<BasicSaccadeCalculator>().As<ISaccadeCalculator>();
            builder.Register((c, p) => {
                var parameters = p as Parameter[] ?? p.ToArray();
                var dataStream = parameters.Named<DataStream>(Constants.DataStreamParameterName);
                return c.Resolve<IGazePointStreamFactory>().GetGazePointStream(dataStream);
            }).As<GazePointStream>().SingleInstance();

            builder.Register((c, p) => {
                var parameters = p as Parameter[] ?? p.ToArray();
                var dataStream = parameters.Named<DataStream>(Constants.DataStreamParameterName);
                var repository = c.Resolve<IGazeDataRepository>(parameters);
                return
                    new GazeDataWriterFactory(repository, c.Resolve<ISaccadeCalculator>()).GetGazeDataWriter(dataStream);
            }).As<IGazeDataWriter>();

            builder.RegisterType<NullGazeDataMonitorFinalizer>().As<IGazeDataMonitorFinalizer>();
            builder.RegisterType<XmlLog4NetLoggerFactory>().As<ILoggerFactory>().SingleInstance();
        }
    }
}
