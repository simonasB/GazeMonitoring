namespace GazeMonitoring.Data.Aggregation.Model
{
    public class SaccadesAggregatedDataByDirectionAndDuration
    {
        public SaccadeDirectionCategory SaccadeDirectionCategory { get; set; }
        public SaccadeDurationCategory SaccadeDurationCategory { get; set; }
        public int Count { get; set; }
        public double RelativePercentage { get; set; }
    }
}
