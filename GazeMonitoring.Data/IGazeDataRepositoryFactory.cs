using GazeMonitoring.Model;

namespace GazeMonitoring.Data
{
    public interface IGazeDataRepositoryFactory
    {
        IGazeDataRepository Create(IMonitoringContext monitoringContext);
    }
}
