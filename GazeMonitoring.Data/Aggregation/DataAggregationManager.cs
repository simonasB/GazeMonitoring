using GazeMonitoring.Data.Aggregation.Aggregators;
using GazeMonitoring.Data.Aggregation.Model;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public interface IDataAggregationManager
    {
        AggregatedData Aggregate(IMonitoringContext monitoringContext);
    }

    public class DataAggregationManager : IDataAggregationManager
    {
        private readonly ICurrentSessionData _currentSessionData;

        public DataAggregationManager(ICurrentSessionData currentSessionData)
        {
            _currentSessionData = currentSessionData;
        }

        public AggregatedData Aggregate(IMonitoringContext monitoringContext)
        {
            var aggregatedData = new AggregatedData();

            // Gaze points
            var mappedGazePointsDataAggregator = new MappedGazePointsDataAggregator(_currentSessionData);
            var gazePointCountsByAoiNameDataAggregator = new GazePointCountsByAoiNameDataAggregator(_currentSessionData);
            var gazePointCountsByScreenConfigurationAndAoiDataAggregator = new GazePointCountsByScreenConfigurationAndAoiDataAggregator(_currentSessionData);

            // Fixation points
            var mappedFixationPointsDataAggregator = new MappedFixationPointsDataAggregator(_currentSessionData);
            var totalFixationTimesAndCountsByAoiNameDataAggregator = new TotalFixationTimesAndCountsByAoiNameDataAggregator(_currentSessionData);
            var fixationTimesAndCountsByScreenConfigurationAndAoiIdDataAggregator = new TotalFixationTimesAndCountsByScreenConfigurationAndAoiIdDataAggregator(_currentSessionData);
            var saccadesDurationByDirectionAggregator = new SaccadesByDirectionAndDurationAggregator(_currentSessionData);


            // Set next aggregator
            mappedGazePointsDataAggregator.SetNext(gazePointCountsByAoiNameDataAggregator);
            gazePointCountsByAoiNameDataAggregator.SetNext(gazePointCountsByScreenConfigurationAndAoiDataAggregator);
            gazePointCountsByScreenConfigurationAndAoiDataAggregator.SetNext(mappedFixationPointsDataAggregator);
            mappedFixationPointsDataAggregator.SetNext(totalFixationTimesAndCountsByAoiNameDataAggregator);
            totalFixationTimesAndCountsByAoiNameDataAggregator.SetNext(fixationTimesAndCountsByScreenConfigurationAndAoiIdDataAggregator);
            fixationTimesAndCountsByScreenConfigurationAndAoiIdDataAggregator.SetNext(saccadesDurationByDirectionAggregator);

            mappedGazePointsDataAggregator.Aggregate(monitoringContext, aggregatedData);

            return aggregatedData;
        }
    }
}
