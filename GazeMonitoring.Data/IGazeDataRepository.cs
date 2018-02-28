using System.Collections.Generic;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data {
    public interface IGazeDataRepository<in TEntity> where TEntity : IGazeData {
        void SaveOne(TEntity entity);
        void SaveMany(IEnumerable<TEntity> entities);
    }
}
