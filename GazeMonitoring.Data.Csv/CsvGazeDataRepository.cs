using System.Collections.Generic;

namespace GazeMonitoring.Data.Csv {
    public class CsvGazeDataRepository : IGazeDataRepository {
        private readonly CsvWritersManager _csvWritersManager;

        public CsvGazeDataRepository(CsvWritersManager csvWritersManager) {
            _csvWritersManager = csvWritersManager;
        }

        public void SaveOne<TEntity>(TEntity entity) {
            _csvWritersManager.GetCsvWriter<TEntity>().WriteRecord(entity);
            _csvWritersManager.GetCsvWriter<TEntity>().NextRecord();
        }

        public void SaveMany<TEntity>(IEnumerable<TEntity> entities) {
            _csvWritersManager.GetCsvWriter<TEntity>().WriteRecords(entities);
        }
    }
}
