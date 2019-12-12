using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public abstract class DataAggregatorBase : IDataAggregator
    {
        protected readonly ICurrentSessionData CurrentSessionData;
        private IDataAggregator _next;

        protected DataAggregatorBase(ICurrentSessionData currentSessionData)
        {
            CurrentSessionData = currentSessionData;
        }

        public virtual void Aggregate(IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            _next?.Aggregate(monitoringContext, aggregatedData);
        }

        public void SetNext(IDataAggregator dataAggregator)
        {
            _next = dataAggregator;
        }
    }
}
