using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public interface IDataAggregator
    {
        void Aggregate(IMonitoringContext monitoringContext, AggregatedData aggregatedData);

        void SetNext(IDataAggregator dataAggregator);
    }
}
