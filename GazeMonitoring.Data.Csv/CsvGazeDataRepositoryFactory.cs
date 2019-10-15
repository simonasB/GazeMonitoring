using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv
{
    public class CsvGazeDataRepositoryFactory : IGazeDataRepositoryFactory
    {
        private readonly ICsvWritersFactory _csvWritersFactory;

        public CsvGazeDataRepositoryFactory(ICsvWritersFactory csvWritersFactory)
        {
            _csvWritersFactory = csvWritersFactory;
        }

        public IGazeDataRepository Create(IMonitoringContext monitoringContext)
        {
            return new CsvGazeDataRepository(_csvWritersFactory.GetCsvWriters(monitoringContext));
        }
    }
}
