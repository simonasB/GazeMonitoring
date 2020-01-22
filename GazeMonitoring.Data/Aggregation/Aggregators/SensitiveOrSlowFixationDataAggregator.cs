using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation.Aggregators
{
    public abstract class SensitiveOrSlowFixationDataAggregator : DataAggregatorBase
    {
        protected SensitiveOrSlowFixationDataAggregator(ICurrentSessionData currentSessionData) : base(currentSessionData)
        {
        }

        protected override bool IsPossibleToAggregate(IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            return monitoringContext.DataStream == DataStream.SlowFixation || monitoringContext.DataStream == DataStream.SensitiveFixation;
        }
    }
}
