﻿using System;
using System.Collections.Generic;
using CsvHelper;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv {
    public class CsvGazeDataRepository : IGazeDataRepository
    {
        private bool _disposed;

        private readonly Dictionary<Type, CsvWriterWrapper> _csvWriterWrappers;

        public CsvGazeDataRepository(Dictionary<Type, CsvWriterWrapper> csvWriterWrappers) {
            if (csvWriterWrappers == null) {
                throw new ArgumentNullException(nameof(csvWriterWrappers));
            }

            _csvWriterWrappers = csvWriterWrappers;
        }

        private CsvWriter GetCsvWriter(Type type) {
            return _csvWriterWrappers[type].CsvWriter;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            foreach (var csvWriterWrapper in _csvWriterWrappers) {
                csvWriterWrapper.Value?.Dispose();
            }
            _disposed = true;
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

        public void SaveFixationPoint(FixationPoint fixationPoint) {
            if (fixationPoint == null) {
                throw new ArgumentNullException(nameof(fixationPoint));
            }

            var csvWriter = GetCsvWriter(typeof(FixationPoint));
            csvWriter.WriteRecord(fixationPoint);
            csvWriter.NextRecord();
        }
    }
}
