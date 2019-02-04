using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv {
    public class CsvWritersFactory : ICsvWritersFactory {
        private readonly IFileNameFormatter _fileNameFormatter;
        private readonly SubjectInfo _subjectInfo;

        public CsvWritersFactory(IFileNameFormatter fileNameFormatter, SubjectInfo subjectInfo) {
            if (fileNameFormatter == null) {
                throw new ArgumentNullException(nameof(fileNameFormatter));
            }

            if (subjectInfo == null) {
                throw new ArgumentNullException(nameof(subjectInfo));
            }

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
                    csvWriters.Add(typeof(FixationPoint), CreateCsvWriter<FixationPoint>(dataStream.ToString()));
                    csvWriters.Add(typeof(Saccade), CreateCsvWriter<Saccade>($"{dataStream}_Saccades"));
                    break;
                case DataStream.SlowFixation:
                    csvWriters.Add(typeof(FixationPoint), CreateCsvWriter<FixationPoint>(dataStream.ToString()));
                    csvWriters.Add(typeof(Saccade), CreateCsvWriter<Saccade>($"{dataStream}_Saccades"));
                    break;
            }

            return csvWriters;
        }

        private CsvWriterWrapper CreateCsvWriter<T>(string dataStream) {
            var fileName = new FileName { DataStream = dataStream, DateTime = DateTime.Now};

            const string csvFolderName = "data_csv";
            var csvFolderPath = Path.Combine(Directory.GetCurrentDirectory(), csvFolderName);

            if (!Directory.Exists(csvFolderPath)) {
                Directory.CreateDirectory(csvFolderPath);
            }

            var textWriter = File.CreateText(Path.Combine(csvFolderPath, _fileNameFormatter.Format(fileName)));
            var csvWriter = new CsvWriter(textWriter);

            Initialize<T>(csvWriter);
            return new CsvWriterWrapper(textWriter, new CsvWriter(textWriter));
        }

        private void Initialize<T>(CsvWriter csvWriter) {
            csvWriter.Configuration.RegisterClassMap<SubjectInfoMap>();

            csvWriter.WriteHeader<SubjectInfo>();
            csvWriter.NextRecord();

            csvWriter.WriteRecord(_subjectInfo);
            csvWriter.NextRecord();

            csvWriter.WriteHeader<T>();
            csvWriter.NextRecord();
        }
    }
}
