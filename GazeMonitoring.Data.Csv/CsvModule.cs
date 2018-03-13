using Autofac;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.Csv {
    public class CsvModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<FileNameFormatter>().As<IFileNameFormatter>();
            builder.RegisterType(typeof(CsvWritersFactory));
            builder.Register((c, p) => new CsvGazeDataRepository(c.Resolve<CsvWritersFactory>(), p.Named<DataStream>(Constants.DataStreamParameterName)))
                .As<IGazeDataRepository>();
        }
    }
}
