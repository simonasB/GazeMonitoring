using GazeMonitoring.IoC;

namespace GazeMonitoring.Data.Csv {
    public class CsvModule : IoCModule {
        public void Load(IoContainerBuilder builder)
        {
            builder.Register<IFileNameFormatter, CsvFileNameFormatter>();
            builder.Register<ICsvWritersFactory, CsvWritersFactory>();
            builder.Register<IGazeDataRepositoryFactory, CsvGazeDataRepositoryFactory>();
        }
    }
}
