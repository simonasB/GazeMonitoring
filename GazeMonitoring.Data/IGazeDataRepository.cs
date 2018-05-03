namespace GazeMonitoring.Data {
    public interface IGazeDataRepository {
        void SaveOne<TEntity>(TEntity entity);
    }
}
