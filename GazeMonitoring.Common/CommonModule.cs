using System.Linq;
using Autofac;
using Autofac.Core;
using GazeMonitoring.Common.Entities;
using GazeMonitoring.Data;

namespace GazeMonitoring.Common {
    public class CommonModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<BasicSaccadeCalculator>().As<ISaccadeCalculator>();
            builder.Register((c, p) => {
                var parameters = p as Parameter[] ?? p.ToArray();
                var dataStream = parameters.Named<DataStream>(Constants.DataStreamParameterName);
                var subjectInfo = parameters.Named<SubjectInfo>(Constants.SubjectInfoParameterName);
                var repository = c.Resolve<IGazeDataRepository>(
                    new NamedParameter(Constants.DataStreamParameterName, dataStream),
                    new NamedParameter(Constants.SubjectInfoParameterName, subjectInfo));
                return
                    new GazeDataMonitor(
                        c.Resolve<IGazePointStreamFactory>().GetGazePointStream(dataStream),
                        new GazeDataWriterFactory(repository, c.Resolve<ISaccadeCalculator>()).GetGazeDataWriter(dataStream));
            }).As<GazeDataMonitor>();
        }
    }
}
