using System;
using System.Collections.Generic;
using CsvHelper;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv {
    public class CsvGazeDataRepository : IGazeDataRepository, IDisposable {
        private readonly Dictionary<Type, CsvWriterWrapper> _csvWriterWrappers;

        public CsvGazeDataRepository(ICsvWritersFactory csvWritersFactory, DataStream dataStream) {
            if (csvWritersFactory == null) {
                throw new ArgumentNullException(nameof(csvWritersFactory));
            }

            _csvWriterWrappers = csvWritersFactory.GetCsvWriters(dataStream);
        }

        public void SaveOne<TEntity>(TEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }

            GetCsvWriter<TEntity>().WriteRecord(entity);
            GetCsvWriter<TEntity>().NextRecord();
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
