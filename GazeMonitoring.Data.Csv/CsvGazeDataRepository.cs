using System.Collections.Generic;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.Csv {
    public class CsvGazeDataRepository<TEntity> : IGazeDataRepository<TEntity> where TEntity : IGazeData {
        private readonly CsvWriterProvider<TEntity> _csvWriterProvider;

        public CsvGazeDataRepository(CsvWriterProvider<TEntity> csvWriterProvider) {
            _csvWriterProvider = csvWriterProvider;
        }

        public void SaveOne(TEntity entity) {
            _csvWriterProvider.CsvWriter.WriteRecord(entity);
            _csvWriterProvider.CsvWriter.NextRecord();
        }

        public void SaveMany(IEnumerable<TEntity> entities) {
            _csvWriterProvider.CsvWriter.WriteRecords(entities);
        }
    }
}
