using System;

namespace GazeMonitoring.Data.Aggregation.Model
{
    public class FixationPointsAggregatedData
    {
        /// <summary>
        /// Can be id or name or anything else to group by which is unique among aoi
        /// </summary>
        public string Identifier { get; set; }
        public string IdentifierReadableName { get; set; }
        public int FixationPointsCount { get; set; }
        public TimeSpan FixationPointsDuration { get; set; }
        public int LongFixationPointsCount { get; set; }
        public int ShortFixationPointsCount { get; set; }
    }
}
