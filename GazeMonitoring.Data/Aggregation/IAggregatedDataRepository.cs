using System.Threading.Tasks;
using GazeMonitoring.Data.Aggregation.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public interface IAggregatedDataRepository
    {
        Task Save(AggregatedData aggregatedData);
    }
}
