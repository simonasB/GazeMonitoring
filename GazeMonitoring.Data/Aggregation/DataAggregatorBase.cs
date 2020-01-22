using GazeMonitoring.Data.Aggregation.Model;
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

        public void Aggregate(IMonitoringContext monitoringContext, AggregatedData aggregatedData)
        {
            if (monitoringContext.MonitoringConfiguration == null)
                return;

            if (!IsPossibleToAggregate(monitoringContext, aggregatedData))
            {
                _next?.Aggregate(monitoringContext, aggregatedData);
                return;
            }

            AggregateInternal(monitoringContext, aggregatedData);

            _next?.Aggregate(monitoringContext, aggregatedData);
        }

        public void SetNext(IDataAggregator dataAggregator)
        {
            _next = dataAggregator;
        }

        protected abstract void AggregateInternal(IMonitoringContext monitoringContext, AggregatedData aggregatedData);

        protected abstract bool IsPossibleToAggregate(IMonitoringContext monitoringContext, AggregatedData aggregatedData);
    }
}
