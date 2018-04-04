using System;
using System.IO;
using CsvHelper;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.PostgreSQL {
    public class PostgreSQLGazeDataMonitorFinalizer : IGazeDataMonitorFinalizer {
        private readonly DatabaseRepository _databaseRepository;
        private readonly SubjectInfo _subjectInfo;

        public PostgreSQLGazeDataMonitorFinalizer(DatabaseRepository databaseRepository, SubjectInfo subjectInfo) {
            _databaseRepository = databaseRepository;
            _subjectInfo = subjectInfo;
        }

        public void FinalizeMonitoring() {
            _databaseRepository.SaveSubjectInfo(_subjectInfo);
            var savedSubjectInfo = _databaseRepository.RetrieveSubjectInfo(_subjectInfo.SessionId);

            if (savedSubjectInfo == null) {
                // Something went wrong. 
            }

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
            }
        }
    }
}
