namespace GazeMonitoring.Data.Aggregation.Model
{
    public class GazePointsAggregatedData
    {
        /// <summary>
        /// Can be id or name or anything else to group by which is unique among aoi
        /// </summary>
        public string Identifier { get; set; }
        public string IdentifierReadableName { get; set; }
        public int PointsCount { get; set; }
    }
}
