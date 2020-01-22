namespace GazeMonitoring.Data.Aggregation.Model
{
    public enum SaccadeDurationCategory
    {
        // < 60 ms
        Short,
        // > 60, <= 200 ms
        Medium,
        // > 200 ms
        Long
    }
}
