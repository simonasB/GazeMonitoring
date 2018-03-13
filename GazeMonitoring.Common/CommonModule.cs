using Autofac;
using GazeMonitoring.Common.Entities;
using GazeMonitoring.Data;

namespace GazeMonitoring.Common {
    public class CommonModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<BasicSaccadeCalculator>().As<ISaccadeCalculator>();
            builder.Register((c, p) => {
                var dataStream = p.Named<DataStream>(Constants.DataStreamParameterName);
                var repository = c.Resolve<IGazeDataRepository>(new NamedParameter(Constants.DataStreamParameterName, dataStream));
                return
                    new GazeDataMonitor(
                        c.Resolve<IGazePointStreamFactory>().GetGazePointStream(dataStream),
                        new GazeDataWriterFactory(repository, c.Resolve<ISaccadeCalculator>()).GetGazeDataWriter(dataStream));
            }).As<GazeDataMonitor>();
        }
    }
}
