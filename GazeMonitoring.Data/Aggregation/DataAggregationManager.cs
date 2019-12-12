﻿using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public interface IDataAggregationManager
    {
        AggregatedData Aggregate(MonitoringConfiguration monitoringConfiguration, IMonitoringContext monitoringContext);
    }

    public class DataAggregationManager : IDataAggregationManager
    {
        private readonly ICurrentSessionData _currentSessionData;

        public DataAggregationManager(ICurrentSessionData currentSessionData)
        {
            _currentSessionData = currentSessionData;
        }

        public AggregatedData Aggregate(MonitoringConfiguration monitoringConfiguration, IMonitoringContext monitoringContext)
        {
            var aggregatedData = new AggregatedData();

            var mappedFixationPointsDataAggregator = new MappedFixationPointsDataAggregator(_currentSessionData);

            // Set next aggregator
            //mappedFixationPointsDataAggregator.SetNext();
            mappedFixationPointsDataAggregator.Aggregate(monitoringContext, aggregatedData);

            return aggregatedData;
        }
    }
}
