using GazeMonitoring.Model;

namespace GazeMonitoring.Data
{
    public abstract class GazeDataRepositoryFactoryBase : IGazeDataRepositoryFactory
    {
        public IGazeDataRepository Create(IMonitoringContext monitoringContext)
        {
            return new MultipleSourceGazeDataRepository(new[] {new TempGazeDataRepository(new TempDataConfiguration()), CreateConcreteRepo(monitoringContext)});
        }

        protected abstract IGazeDataRepository CreateConcreteRepo(IMonitoringContext monitoringContext);
    }
}
