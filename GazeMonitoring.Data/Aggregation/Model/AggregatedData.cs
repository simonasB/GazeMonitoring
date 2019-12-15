using System.Collections.Generic;

namespace GazeMonitoring.Data.Aggregation.Model
{
    public class AggregatedData
    {
        public List<MappedFixationPoint> MappedFixationPoints { get; set; }
        public List<FixationPointsAggregatedDataForScreenConfiguration> FixationPointsAggregatedDataForScreenConfigurations { get; set; }
        public List<FixationPointsAggregatedDataForAoi> FixationPointsAggregatedDataForAoiByName{ get; set; }
    }
}
