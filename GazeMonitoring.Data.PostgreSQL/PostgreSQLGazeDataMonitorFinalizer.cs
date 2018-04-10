using System;
using System.IO;
using CsvHelper;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;
using GazeMonitoring.Logging;

namespace GazeMonitoring.Data.PostgreSQL {
    public class PostgreSQLGazeDataMonitorFinalizer : IGazeDataMonitorFinalizer {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly SubjectInfo _subjectInfo;
        private ILogger _logger;

        public PostgreSQLGazeDataMonitorFinalizer(IDatabaseRepository databaseRepository, SubjectInfo subjectInfo, ILoggerFactory loggerFactory) {
            _logger = loggerFactory.GetLogger(typeof(PostgreSQLGazeDataMonitorFinalizer));
            _databaseRepository = databaseRepository;
            _subjectInfo = subjectInfo;
        }

        public void FinalizeMonitoring() {
            _logger.Information("Starting finalization process of gaze data monitoring.");

            _databaseRepository.SaveSubjectInfo(_subjectInfo);
            var savedSubjectInfo = _databaseRepository.RetrieveSubjectInfo(_subjectInfo.SessionId);

            var saccadesFilePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.SaccadesTempCsvFileName);
            var gazePointsFilePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.GazePointsTempCsvFileName);

            if (File.Exists(gazePointsFilePath)) {
                using (TextReader reader = File.OpenText(gazePointsFilePath)) {
                    // Skip SubjectInfo header
                    reader.ReadLine();
                    // Skip SubjectInfo data
                    reader.ReadLine();
                    // now initialize the CsvReader
                    var parser = new CsvReader(reader);
                    var gazePoints = parser.GetRecords<GazePoint>();

                    _databaseRepository.BinaryInsertGazePoints(gazePoints, _subjectInfo.SessionId, savedSubjectInfo?.Id, DateTime.UtcNow);
                }

                File.Delete(gazePointsFilePath);
            }

            if (File.Exists(saccadesFilePath)) {
                using (TextReader reader = File.OpenText(saccadesFilePath)) {
                    // Skip SubjectInfo header
                    reader.ReadLine();
                    // Skip SubjectInfo data
                    reader.ReadLine();
                    // now initialize the CsvReader
                    var parser = new CsvReader(reader);
                    var saccades = parser.GetRecords<Saccade>();

                    _databaseRepository.BinaryInsertSaccades(saccades, _subjectInfo.SessionId, savedSubjectInfo?.Id, DateTime.UtcNow);
                }

                File.Delete(saccadesFilePath);
            }

            _logger.Information("Finalization process of gaze data monitoring ended.");
        }
    }
}
