using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using GazeMonitoring.Common.Misc;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv {
    public class CsvWritersFactory {
        private readonly IFileNameFormatter _fileNameFormatter;
        private readonly SubjectInfo _subjectInfo;

        public CsvWritersFactory(IFileNameFormatter fileNameFormatter, SubjectInfo subjectInfo) {
            _fileNameFormatter = fileNameFormatter;
            _subjectInfo = subjectInfo;
        }

        public Dictionary<Type, CsvWriterWrapper> GetCsvWriters(DataStream dataStream) {
            var csvWriters = new Dictionary<Type, CsvWriterWrapper>();

            switch (dataStream) {
                case DataStream.UnfilteredGaze:
                    csvWriters.Add(typeof(GazePoint), CreateCsvWriter<GazePoint>(dataStream.ToString()));
                    break;
                case DataStream.LightlyFilteredGaze:
                    csvWriters.Add(typeof(GazePoint), CreateCsvWriter<GazePoint>(dataStream.ToString()));
                    break;
                case DataStream.SensitiveFixation:
                    csvWriters.Add(typeof(GazePoint), CreateCsvWriter<GazePoint>(dataStream.ToString()));
                    csvWriters.Add(typeof(Saccade), CreateCsvWriter<Saccade>($"{dataStream}_Saccades"));
                    break;
                case DataStream.SlowFixation:
                    csvWriters.Add(typeof(GazePoint), CreateCsvWriter<GazePoint>(dataStream.ToString()));
                    csvWriters.Add(typeof(Saccade), CreateCsvWriter<Saccade>($"{dataStream}_Saccades"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }

            return csvWriters;
        }

        private CsvWriterWrapper CreateCsvWriter<T>(string dataStream) {
            var fileName = new FileName { DataStream = dataStream };
            var textWriter = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), _fileNameFormatter.Format(fileName)));
            var csvWriter = new CsvWriter(textWriter);

            Initialize<T>(csvWriter);
            return new CsvWriterWrapper(textWriter, new CsvWriter(textWriter));
        }

        private void Initialize<T>(CsvWriter csvWriter) {
            csvWriter.WriteHeader<SubjectInfo>();
            csvWriter.NextRecord();

            csvWriter.WriteRecord(_subjectInfo);
            csvWriter.NextRecord();

            csvWriter.WriteHeader<T>();
            csvWriter.NextRecord();
        }
    }
}
