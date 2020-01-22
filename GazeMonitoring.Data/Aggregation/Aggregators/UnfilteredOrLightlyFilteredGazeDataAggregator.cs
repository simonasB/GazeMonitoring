using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation.Aggregators
{
    public abstract class UnfilteredOrLightlyFilteredGazeDataAggregator : DataAggregatorBase
    {
        protected UnfilteredOrLightlyFilteredGazeDataAggregator(ICurrentSessionData currentSessionData) : base(currentSessionData)
        {
        }

        protected override bool IsPossibleToAggregate(IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            return monitoringContext.MonitoringConfiguration != null &&
                   monitoringContext.DataStream == DataStream.LightlyFilteredGaze ||
                   monitoringContext.DataStream == DataStream.UnfilteredGaze;
        }
    }
}
