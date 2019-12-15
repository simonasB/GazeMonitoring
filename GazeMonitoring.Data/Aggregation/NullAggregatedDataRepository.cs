using System.Threading.Tasks;
using GazeMonitoring.Data.Aggregation.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public class NullAggregatedDataRepository : IAggregatedDataRepository
    {
        public Task Save(AggregatedData aggregatedData)
        {
            return Task.CompletedTask;
        }
    }
}
