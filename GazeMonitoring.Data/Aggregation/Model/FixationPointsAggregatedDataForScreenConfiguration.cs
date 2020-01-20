using System.Collections.Generic;

namespace GazeMonitoring.Data.Aggregation.Model
{
    public class FixationPointsAggregatedDataForScreenConfiguration : FixationPointsAggregatedData
    {
        public List<FixationPointsAggregatedDataForAoi> AggregatedDataForAois { get; set; }
    }
}
