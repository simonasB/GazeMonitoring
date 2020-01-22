using System.Threading.Tasks;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public interface IAggregatedDataRepository
    {
        Task Save(AggregatedData aggregatedData, IMonitoringContext monitoringContext);
    }
}
