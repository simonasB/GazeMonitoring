using System.Linq;
using Autofac;
using Autofac.Core;
using GazeMonitoring.Common.Calculations;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.Common.Writers;
using GazeMonitoring.Data;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Logging;
using GazeMonitoring.Model;

namespace GazeMonitoring.Common.IoC {
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
                var subjectInfo = parameters.Named<SubjectInfo>(Constants.SubjectInfoParameterName);
                var repository = c.Resolve<IGazeDataRepository>(
                    new NamedParameter(Constants.DataStreamParameterName, dataStream),
                    new NamedParameter(Constants.SubjectInfoParameterName, subjectInfo));
                return
                    new GazeDataMonitor(
                        c.Resolve<GazePointStream>(new NamedParameter(Constants.DataStreamParameterName, dataStream)),
                        new GazeDataWriterFactory(repository, c.Resolve<ISaccadeCalculator>()).GetGazeDataWriter(dataStream));
            }).As<GazeDataMonitor>();

            builder.RegisterType<NullGazeDataMonitorFinalizer>().As<IGazeDataMonitorFinalizer>();
            builder.RegisterType<XmlLog4NetLoggerFactory>().As<ILoggerFactory>().SingleInstance();
        }
    }
}
