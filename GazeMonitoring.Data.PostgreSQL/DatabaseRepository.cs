using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using GazeMonitoring.Common.Entities;
using Npgsql;
using NpgsqlTypes;

namespace GazeMonitoring.Data.PostgreSQL {
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _connectionString;

        private static readonly string _subjectInfoTableName = "gaze_monitoring.subject_info";

        private static readonly string[] _allSubjectInfoColumns = {
            "session_id", "name", "age", "details", "id"
        };

        private static readonly string[] _subjectInfoColumnsForSave = {
            "session_id", "name", "age", "details"
        };

        private static readonly string _gazePointTableName = "gaze_monitoring.gaze_point";

        private static readonly string[] _gazePointsColumn = {
            "X", "y", "timestamp", "session_id", "subject_info_id", "sample_time"
        };

        private static readonly string _saccadeTableName = "gaze_monitoring.saccade";

        private static readonly string[] _saccadeColumns = {
            "direction", "amplitude", "velocity", "start_timestamp", "end_timestamp", "session_id", "subject_info_id", "sample_time"
        };

        public DatabaseRepository(string connectionString) {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public SubjectInfo RetrieveSubjectInfo(string sessionId) {
            using (var connection = Common.CreateConnection(_connectionString))
            {
                string columns = string.Join(",", _allSubjectInfoColumns);
                string commandText = $"SELECT {columns} FROM {_subjectInfoTableName} " +
                                     "WHERE session_id=@session_id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@session_id", NpgsqlDbType.Uuid, sessionId);

                    try
                    {
                        using (DbDataReader reader = command.ExecuteReader()) {
                            if (reader.Read()) {
                                return new SubjectInfo {
                                    SessionId = reader.IsDBNull(0) ? null : Convert.ToString(reader[0]),
                                    Name = reader.IsDBNull(1) ? null : Convert.ToString(reader[1]),
                                    Age = reader.IsDBNull(2) ? (int?) null : Convert.ToInt32(reader[2]),
                                    Details = reader.IsDBNull(3) ? null : Convert.ToString(reader[3]),
                                    Id = reader.IsDBNull(4) ? (int?) null : Convert.ToInt32(reader[4])
                                };
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // log error
                    }
                }
            }
            return null;
        }

        public void SaveSubjectInfo(SubjectInfo subjectInfo) {
            string columns = string.Join(",", _subjectInfoColumnsForSave);
            string values = string.Join(",", _subjectInfoColumnsForSave.Select(c => "@" + c));
            string insertText = $"INSERT INTO {_subjectInfoTableName} ({columns}) VALUES ({values})";
            using (var connection = Common.CreateConnection(_connectionString)) {
                using (var command = new NpgsqlCommand(insertText, connection)) {
                    command.Parameters.Add(new NpgsqlParameter("@session_id", NpgsqlDbType.Uuid));
                    command.Parameters.Add(new NpgsqlParameter("@name", NpgsqlDbType.Text));
                    command.Parameters.Add(new NpgsqlParameter("@age", NpgsqlDbType.Integer));
                    command.Parameters.Add(new NpgsqlParameter("@details", NpgsqlDbType.Text));

                    command.Parameters[0].Value = Common.ReplaceNullToDbNull(subjectInfo.SessionId);
                    command.Parameters[1].Value = Common.ReplaceNullToDbNull(subjectInfo.Name);
                    command.Parameters[2].Value = Common.ReplaceNullToDbNull(subjectInfo.Age);
                    command.Parameters[3].Value = Common.ReplaceNullToDbNull(subjectInfo.Details);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void BinaryInsertGazePoints(IEnumerable<GazePoint> gazePoints, string sessionId, int? subjectInfoId, DateTime sampleTime) {
            using (var connection = Common.CreateConnection(_connectionString)) {
                var partitionName = Common.GetPartitionName(sampleTime);

                string copyFromCommand = $"COPY {_gazePointTableName}_{partitionName} ({string.Join(",", _gazePointsColumn)}) FROM STDIN (FORMAT BINARY)";

                using (NpgsqlBinaryImporter npgsqlBinaryImporter = connection.BeginBinaryImport(copyFromCommand)) {
                    foreach (var gazePoint in gazePoints) {
                        npgsqlBinaryImporter.StartRow();
                        npgsqlBinaryImporter.Write(gazePoint.X, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(gazePoint.Y, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(gazePoint.Timestamp, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(sessionId, NpgsqlDbType.Uuid);
                        npgsqlBinaryImporter.Write(subjectInfoId, NpgsqlDbType.Integer);
                        npgsqlBinaryImporter.Write(sampleTime, NpgsqlDbType.Timestamp);
                    }
                }
            }
        }

        public void BinaryInsertSaccades(IEnumerable<Saccade> saccades, string sessionId, int? subjectInfoId, DateTime sampleTime) {
            using (var connection = Common.CreateConnection(_connectionString)) {
                var partitionName = Common.GetPartitionName(sampleTime);

                string copyFromCommand = $"COPY {_saccadeTableName}_{partitionName} ({string.Join(",", _saccadeColumns)}) FROM STDIN (FORMAT BINARY)";

                using (NpgsqlBinaryImporter npgsqlBinaryImporter = connection.BeginBinaryImport(copyFromCommand)) {
                    foreach (var saccade in saccades) {
                        npgsqlBinaryImporter.StartRow();
                        npgsqlBinaryImporter.Write(saccade.Direction, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(saccade.Amplitude, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(saccade.Velocity, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(saccade.StartTimeStamp, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(saccade.EndTimeStamp, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(sessionId, NpgsqlDbType.Uuid);
                        npgsqlBinaryImporter.Write(subjectInfoId, NpgsqlDbType.Integer);
                        npgsqlBinaryImporter.Write(sampleTime, NpgsqlDbType.Timestamp);
                    }
                }
            }
        }
    }
}
