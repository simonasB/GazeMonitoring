using System.Collections.Generic;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public interface IDataAggregationManager
    {
        AggregatedData Aggregate(MonitoringConfiguration monitoringConfiguration, IMonitoringContext monitoringContext);
    }

    public class DataAggregationManager : IDataAggregationManager
    {
        private readonly IEnumerable<IDataAggregator> _dataAggregators;

        public DataAggregationManager(IEnumerable<IDataAggregator> dataAggregators)
        {
            _dataAggregators = dataAggregators;
        }

        public AggregatedData Aggregate(MonitoringConfiguration monitoringConfiguration, IMonitoringContext monitoringContext)
        {
            var aggregatedData = new AggregatedData();

            foreach (var dataAggregator in _dataAggregators)
            {
                dataAggregator.Aggregate(monitoringConfiguration, monitoringContext, aggregatedData);
            }

            return aggregatedData;
        }
    }
}
