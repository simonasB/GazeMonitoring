using System;

namespace GazeMonitoring.Data.Aggregation.Model
{
    public class FixationPointsAggregatedDataForAoi
    {
        /// <summary>
        /// Can be id or name or anything else to group by which is unique among aoi
        /// </summary>
        public string Identifier { get; set; }
        public int FixationPointsCount { get; set; }
        public TimeSpan FixationPointsDuration { get; set; }
    }
}
