using System.Threading.Tasks;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public class NullAggregatedDataRepository : IAggregatedDataRepository
    {
        public Task Save(AggregatedData aggregatedData, IMonitoringContext monitoringContext)
        {
            return Task.CompletedTask;
        }
    }
}
