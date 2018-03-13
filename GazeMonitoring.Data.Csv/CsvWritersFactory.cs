using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.Csv {
    public class CsvWritersFactory {
        private readonly IFileNameFormatter _fileNameFormatter;

        public CsvWritersFactory(IFileNameFormatter fileNameFormatter) {
            _fileNameFormatter = fileNameFormatter;
        }

        public Dictionary<Type, CsvWriterWrapper> GetCsvWriters(DataStream dataStream) {
            var csvWriters = new Dictionary<Type, CsvWriterWrapper>();

            switch (dataStream) {
                case DataStream.UnfilteredGaze:
                    csvWriters.Add(typeof(GazePoint), CreateCsvWriter(dataStream.ToString()));
                    break;
                case DataStream.LightlyFilteredGaze:
                    csvWriters.Add(typeof(GazePoint), CreateCsvWriter(dataStream.ToString()));
                    break;
                case DataStream.SensitiveFixation:
                    csvWriters.Add(typeof(GazePoint), CreateCsvWriter(dataStream.ToString()));
                    csvWriters.Add(typeof(Saccade), CreateCsvWriter($"{dataStream}_Saccades"));
                    break;
                case DataStream.SlowFixation:
                    csvWriters.Add(typeof(GazePoint), CreateCsvWriter(dataStream.ToString()));
                    csvWriters.Add(typeof(Saccade), CreateCsvWriter($"{dataStream}_Saccades"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }

            return csvWriters;
        }

        private CsvWriterWrapper CreateCsvWriter(string dataStream) {
            var fileName = new FileName { DataStream = dataStream };
            var textWriter = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), _fileNameFormatter.Format(fileName)));
            return new CsvWriterWrapper(textWriter, new CsvWriter(textWriter));
        }
    }
}
