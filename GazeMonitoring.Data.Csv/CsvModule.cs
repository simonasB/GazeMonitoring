using Autofac;

namespace GazeMonitoring.Data.Csv {
    public class CsvModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<CsvFileNameFormatter>().As<IFileNameFormatter>();
            builder.RegisterType<CsvWritersFactory>().As<ICsvWritersFactory>();
            builder.RegisterType<CsvGazeDataRepositoryFactory>().As<IGazeDataRepositoryFactory>();
        }
    }
}
