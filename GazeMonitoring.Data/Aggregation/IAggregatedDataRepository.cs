namespace GazeMonitoring.Data.Aggregation
{
    public interface IAggregatedDataRepository
    {
        void Save(AggregatedData aggregatedData);
    }
}
