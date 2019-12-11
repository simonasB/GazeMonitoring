using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public abstract class DataAggregatorBase : IDataAggregator
    {
        protected readonly ICurrentSessionData CurrentSessionData;

        protected DataAggregatorBase(ICurrentSessionData currentSessionData)
        {
            CurrentSessionData = currentSessionData;
        }

        public abstract void Aggregate(MonitoringConfiguration monitoringConfiguration, IMonitoringContext monitoringContext, AggregatedData aggregatedData);
    }
}
