using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public class DataAggregator : DataAggregatorBase
    {

        public override void Aggregate(MonitoringConfiguration monitoringConfiguration, IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            // Set some property of aggregated data
        }

        public DataAggregator(ICurrentSessionData currentSessionData) : base(currentSessionData)
        {
        }
    }
}
