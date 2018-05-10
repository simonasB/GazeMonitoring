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

        private CsvWriter GetCsvWriter(Type type) {
            return _csvWriterWrappers[type].CsvWriter;
        }

        public void Dispose() {
            foreach (var csvWriterWrapper in _csvWriterWrappers) {
                csvWriterWrapper.Value?.Dispose();
            }
        }

        public void SaveGazePoint(GazePoint gazePoint) {
            if (gazePoint == null) {
                throw new ArgumentNullException(nameof(gazePoint));
            }

            var csvWriter = GetCsvWriter(typeof(GazePoint));
            csvWriter.WriteRecord(gazePoint);
            csvWriter.NextRecord();
        }

        public void SaveSaccade(Saccade saccade) {
            if (saccade == null) {
                throw new ArgumentNullException(nameof(saccade));
            }

            var csvWriter = GetCsvWriter(typeof(Saccade));
            csvWriter.WriteRecord(saccade);
            csvWriter.NextRecord();
        }
    }
}
