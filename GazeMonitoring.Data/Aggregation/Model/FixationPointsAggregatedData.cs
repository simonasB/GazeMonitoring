using System;

namespace GazeMonitoring.Data.Aggregation.Model
{
    public class FixationPointsAggregatedData : GazePointsAggregatedData
    {
        public TimeSpan FixationPointsDuration { get; set; }
        public int LongFixationPointsCount { get; set; }
        public int ShortFixationPointsCount { get; set; }
    }
}
