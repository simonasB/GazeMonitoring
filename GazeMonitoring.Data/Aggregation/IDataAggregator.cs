using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public interface IDataAggregator
    {
        void Aggregate(MonitoringConfiguration monitoringConfiguration, IMonitoringContext monitoringContext, AggregatedData aggregatedData);
    }
}
