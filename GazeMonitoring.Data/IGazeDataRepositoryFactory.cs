using GazeMonitoring.Model;

namespace GazeMonitoring.Data
{
    public interface IGazeDataRepositoryFactory
    {
        IGazeDataRepository Create(IMonitoringContext monitoringContext);
    }

    public class NullGazeDataRepositoryFactory : IGazeDataRepositoryFactory
    {
        public IGazeDataRepository Create(IMonitoringContext monitoringContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
