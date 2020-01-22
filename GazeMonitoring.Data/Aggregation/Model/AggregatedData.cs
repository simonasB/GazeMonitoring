using System.Collections.Generic;

namespace GazeMonitoring.Data.Aggregation.Model
{
    public class AggregatedData
    {
        public List<MappedGazePoint> MappedGazePoints { get; set; }
        public List<GazePointsAggregateDataForAoi> GazePointsAggregateDataForAoiByName { get; set; }
        public List<GazePointsAggregatedDataForScreenConfiguration> GazePointsAggregateDataForScreenConfigurations { get; set; }
        public List<MappedFixationPoint> MappedFixationPoints { get; set; }
        public List<FixationPointsAggregatedDataForScreenConfiguration> FixationPointsAggregatedDataForScreenConfigurations { get; set; }
        public List<FixationPointsAggregatedDataForAoi> FixationPointsAggregatedDataForAoiByName { get; set; }
        public List<SaccadesAggregatedDataByDirectionAndDuration> SaccadesAggregatedDataByDirectionAndDuration { get; set; }
    }
}
