using System;
using System.Collections.Generic;
using CsvHelper;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv {
    public class CsvGazeDataRepository : IGazeDataRepository, IDisposable {
        private readonly Dictionary<Type, CsvWriterWrapper> _csvWriterWrappers;

        public CsvGazeDataRepository(CsvWritersFactory csvWritersFactory, DataStream dataStream) {
            _csvWriterWrappers = csvWritersFactory.GetCsvWriters(dataStream);
        }

        public void SaveOne<TEntity>(TEntity entity) {
            GetCsvWriter<TEntity>().WriteRecord(entity);
            GetCsvWriter<TEntity>().NextRecord();
        }

        public void SaveMany<TEntity>(IEnumerable<TEntity> entities) {
            GetCsvWriter<TEntity>().WriteRecords(entities);
        }

        private CsvWriter GetCsvWriter<T>() {
            return _csvWriterWrappers[typeof(T)].CsvWriter;
        }

        public void Dispose() {
            foreach (var csvWriterWrapper in _csvWriterWrappers) {
                csvWriterWrapper.Value?.Dispose();
            }
        }
    }
}
