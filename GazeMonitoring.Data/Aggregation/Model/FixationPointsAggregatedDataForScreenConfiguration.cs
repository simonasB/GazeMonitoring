using System;
using System.Collections.Generic;

namespace GazeMonitoring.Data.Aggregation.Model
{
    public class FixationPointsAggregatedDataForScreenConfiguration
    {
        public string ScreenConfigurationId { get; set; }
        public List<FixationPointsAggregatedDataForAoi> AggregatedDataForAois { get; set; }
        public int FixationPointsCount { get; set; }
        public TimeSpan FixationPointsDuration { get; set; }
        public int LongFixationPointsCount { get; set; }
        public int ShortFixationPointsCount { get; set; }
    }
}
