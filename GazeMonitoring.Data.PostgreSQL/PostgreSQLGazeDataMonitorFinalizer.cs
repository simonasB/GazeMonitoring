using System;
using System.IO;
using CsvHelper;
using GazeMonitoring.Common.Finalizers;
using GazeMonitoring.Logging;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.PostgreSQL {
    public class PostgreSQLGazeDataMonitorFinalizer : IGazeDataMonitorFinalizer {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly ILogger _logger;

        public PostgreSQLGazeDataMonitorFinalizer(IDatabaseRepository databaseRepository, ILoggerFactory loggerFactory) {
            if (databaseRepository == null) {
                throw new ArgumentNullException(nameof(databaseRepository));
            }

            if (loggerFactory == null) {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.GetLogger(typeof(PostgreSQLGazeDataMonitorFinalizer));
            _databaseRepository = databaseRepository;
        }

        public void FinalizeMonitoring(IMonitoringContext monitoringContext) {
            _logger.Information("Starting finalization process of gaze data monitoring.");

            _databaseRepository.SaveSubjectInfo(monitoringContext.SubjectInfo);
            var savedSubjectInfo = _databaseRepository.RetrieveSubjectInfo(monitoringContext.SubjectInfo.SessionId);

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
                    parser.Configuration.ReadingExceptionOccurred = null;
                    parser.Configuration.BadDataFound = null;
                    parser.Configuration.MissingFieldFound = null;

                    var gazePoints = parser.GetRecords<GazePoint>();

                    _databaseRepository.BinaryInsertGazePoints(gazePoints, monitoringContext.SubjectInfo.SessionId, savedSubjectInfo?.Id, DateTime.UtcNow);
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
                    parser.Configuration.ReadingExceptionOccurred = null;
                    parser.Configuration.BadDataFound = null;
                    parser.Configuration.MissingFieldFound = null;

                    var saccades = parser.GetRecords<Saccade>();

                    _databaseRepository.BinaryInsertSaccades(saccades, monitoringContext.SubjectInfo.SessionId, savedSubjectInfo?.Id, DateTime.UtcNow);
                }

                File.Delete(saccadesFilePath);
            }

            _logger.Information("Finalization process of gaze data monitoring ended.");
        }
    }
}
