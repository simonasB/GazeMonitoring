using System.Collections.Generic;

namespace GazeMonitoring.Data.Aggregation.Model
{
    public class GazePointsAggregatedDataForScreenConfiguration : GazePointsAggregatedData
    {
        public List<GazePointsAggregateDataForAoi> AggregatedDataForAois { get; set; }
    }
}
