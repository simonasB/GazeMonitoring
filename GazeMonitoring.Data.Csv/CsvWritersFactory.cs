using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Csv {
    public class CsvWritersFactory : ICsvWritersFactory {
        private readonly IFileNameFormatter _fileNameFormatter;

        public CsvWritersFactory(IFileNameFormatter fileNameFormatter) {
            if (fileNameFormatter == null) {
                throw new ArgumentNullException(nameof(fileNameFormatter));
            }

            _fileNameFormatter = fileNameFormatter;
        }

        public Dictionary<Type, CsvWriterWrapper> GetCsvWriters(IMonitoringContext monitoringContext) {
            var csvWriters = new Dictionary<Type, CsvWriterWrapper>();
            var dataStream = monitoringContext.DataStream;
            var subjectInfo = monitoringContext.SubjectInfo;

            switch (dataStream) {
                case DataStream.UnfilteredGaze:
                    csvWriters.Add(typeof(GazePoint), CreateCsvWriter<GazePoint>(dataStream.ToString(), subjectInfo));
                    break;
                case DataStream.LightlyFilteredGaze:
                    csvWriters.Add(typeof(GazePoint), CreateCsvWriter<GazePoint>(dataStream.ToString(), subjectInfo));
                    break;
                case DataStream.SensitiveFixation:
                    csvWriters.Add(typeof(FixationPoint), CreateCsvWriter<FixationPoint>(dataStream.ToString(), subjectInfo));
                    csvWriters.Add(typeof(Saccade), CreateCsvWriter<Saccade>($"{dataStream}_Saccades", subjectInfo));
                    break;
                case DataStream.SlowFixation:
                    csvWriters.Add(typeof(FixationPoint), CreateCsvWriter<FixationPoint>(dataStream.ToString(), subjectInfo));
                    csvWriters.Add(typeof(Saccade), CreateCsvWriter<Saccade>($"{dataStream}_Saccades", subjectInfo));
                    break;
            }

            return csvWriters;
        }

        private CsvWriterWrapper CreateCsvWriter<T>(string dataStream, SubjectInfo subjectInfo) {
            var fileName = new FileName { DataStream = dataStream, DateTime = DateTime.Now};

            const string csvFolderName = "data_csv";
            var csvFolderPath = Path.Combine(Directory.GetCurrentDirectory(), csvFolderName);

            if (!Directory.Exists(csvFolderPath)) {
                Directory.CreateDirectory(csvFolderPath);
            }

            var textWriter = File.CreateText(Path.Combine(csvFolderPath, _fileNameFormatter.Format(fileName)));
            var csvWriter = new CsvWriter(textWriter);

            Initialize<T>(csvWriter, subjectInfo);
            return new CsvWriterWrapper(textWriter, new CsvWriter(textWriter));
        }

        private void Initialize<T>(CsvWriter csvWriter, SubjectInfo subjectInfo) {
            csvWriter.Configuration.RegisterClassMap<SubjectInfoMap>();

            csvWriter.WriteHeader<SubjectInfo>();
            csvWriter.NextRecord();

            csvWriter.WriteRecord(subjectInfo);
            csvWriter.NextRecord();

            csvWriter.WriteHeader<T>();
            csvWriter.NextRecord();
        }
    }
}
