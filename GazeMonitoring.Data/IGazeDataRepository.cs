using System.Collections.Generic;

namespace GazeMonitoring.Data {
    public interface IGazeDataRepository {
        void SaveOne<TEntity>(TEntity entity);
        void SaveMany<TEntity>(IEnumerable<TEntity> entities);
    }
}
