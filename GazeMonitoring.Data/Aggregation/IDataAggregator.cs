using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public interface IDataAggregator
    {
        void Aggregate(IMonitoringContext monitoringContext, AggregatedData aggregatedData);
    }
}
