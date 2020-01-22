using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv
{
    public class CsvGazeDataRepositoryFactory : GazeDataRepositoryFactoryBase
    {
        private readonly ICsvWritersFactory _csvWritersFactory;

        public CsvGazeDataRepositoryFactory(ICsvWritersFactory csvWritersFactory)
        {
            _csvWritersFactory = csvWritersFactory;
        }

        protected override IGazeDataRepository CreateConcreteRepo(IMonitoringContext monitoringContext)
        {
            return new CsvGazeDataRepository(_csvWritersFactory.GetCsvWriters(monitoringContext));
        }
    }
}
