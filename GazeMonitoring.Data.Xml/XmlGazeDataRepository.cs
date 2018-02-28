using System.Collections.Generic;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.Xml {
    public class XmlGazeDataRepository<TEntity> : IGazeDataRepository<TEntity> where TEntity : IGazeData {
        public void SaveMany(IEnumerable<TEntity> entities) {
            throw new System.NotImplementedException();
        }

        public void SaveOne(TEntity entity) {
            throw new System.NotImplementedException();
        }
    }
}
