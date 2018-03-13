using System.Collections.Generic;
using GazeMonitoring.Common.Entities;
using MongoDB.Driver;

namespace GazeMonitoring.Data.MongoDB {
    public class MongoDBGazeDataRepository : IGazeDataRepository {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoDBGazeDataRepository(IMongoDatabase mongoDatabase) {
            _mongoDatabase = mongoDatabase;
        }
        public void SaveMany<TEntity>(IEnumerable<TEntity> entities)
        {
            GetCollection<TEntity>().InsertMany(entities);
        }

        public void SaveOne<TEntity>(TEntity entity)
        {
            GetCollection<TEntity>().InsertOne(entity);
        }

        private IMongoCollection<T> GetCollection<T>() {
            return _mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public void Dispose() {
            throw new System.NotImplementedException();
        }
    }
}
