using System;
using System.Collections.Generic;
using CsvHelper;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.Csv {
    public class CsvWritersManager : IDisposable {
        private readonly Dictionary<Type, CsvWriterWrapper> _csvWriterWrappers;

        public CsvWritersManager(CsvWritersFactory csvWritersFactory, DataStream dataStream) {
            _csvWriterWrappers = csvWritersFactory.GetCsvWriterProvider(dataStream);
        }

        public CsvWriter GetCsvWriter<T>() {
            return _csvWriterWrappers[typeof(T)].CsvWriter;
        }

        public void Dispose() {
            foreach (var csvWriterWrapper in _csvWriterWrappers) {
                csvWriterWrapper.Value?.Dispose();
            }
        }
    }
}
