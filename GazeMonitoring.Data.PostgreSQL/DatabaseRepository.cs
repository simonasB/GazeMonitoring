using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using GazeMonitoring.Logging;
using GazeMonitoring.Model;
using Npgsql;
using NpgsqlTypes;

namespace GazeMonitoring.Data.PostgreSQL {
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _connectionString;

        private static readonly string[] _allSubjectInfoColumns = {
            "session_id", "name", "age", "details", "id", "session_start_timestamp", "session_end_timestamp"
        };

        private static readonly string[] _subjectInfoColumnsForSave = {
            "session_id", "name", "age", "details", "session_start_timestamp", "session_end_timestamp"
        };

        private static readonly string[] _gazePointsColumn = {
            "X", "y", "timestamp", "session_id", "subject_info_id", "sample_time"
        };

        private static readonly string[] _saccadeColumns = {
            "direction", "amplitude", "velocity", "start_timestamp", "end_timestamp", "session_id", "subject_info_id", "sample_time"
        };

        private readonly ILogger _logger;

        public DatabaseRepository(string connectionString, ILoggerFactory loggerFactory) {
            if (connectionString == null) {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (loggerFactory == null) {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _connectionString = connectionString;
            _logger = loggerFactory.GetLogger(typeof(DatabaseRepository));
        }

        public SubjectInfo RetrieveSubjectInfo(string sessionId) {
            if (sessionId == null) {
                throw new ArgumentNullException(nameof(sessionId));
            }

            using (var connection = Common.CreateConnection(_connectionString)) {
                string columns = string.Join(",", _allSubjectInfoColumns);
                string commandText = $"SELECT {columns} FROM {Constants.SubjectInfoTableName} " +
                                     "WHERE session_id=@session_id";
                using (var command = new NpgsqlCommand(commandText, connection)) {
                    command.Parameters.AddWithValue("@session_id", NpgsqlDbType.Uuid, sessionId);

                    try {
                        using (DbDataReader reader = command.ExecuteReader()) {
                            if (reader.Read()) {
                                return new SubjectInfo {
                                    SessionId = reader.IsDBNull(0) ? null : Convert.ToString(reader[0]),
                                    Name = reader.IsDBNull(1) ? null : Convert.ToString(reader[1]),
                                    Age = reader.IsDBNull(2) ? (int?) null : Convert.ToInt32(reader[2]),
                                    Details = reader.IsDBNull(3) ? null : Convert.ToString(reader[3]),
                                    Id = reader.IsDBNull(4) ? (int?) null : Convert.ToInt32(reader[4]),
                                    SessionStartTimestamp = Convert.ToDateTime(reader[5]),
                                    SessionEndTimeStamp = Convert.ToDateTime(reader[6])
                                };
                            }
                        }
                    } catch (Exception e) {
                        _logger.Error(e);
                    }
                }
            }
            return null;
        }

        public void SaveSubjectInfo(SubjectInfo subjectInfo) {
            if (subjectInfo == null) {
                throw new ArgumentNullException(nameof(subjectInfo));
            }

            string columns = string.Join(",", _subjectInfoColumnsForSave);
            string values = string.Join(",", _subjectInfoColumnsForSave.Select(c => "@" + c));
            string insertText = $"INSERT INTO {Constants.SubjectInfoTableName} ({columns}) VALUES ({values})";
            using (var connection = Common.CreateConnection(_connectionString)) {
                using (var command = new NpgsqlCommand(insertText, connection)) {
                    command.Parameters.Add(new NpgsqlParameter("@session_id", NpgsqlDbType.Uuid));
                    command.Parameters.Add(new NpgsqlParameter("@name", NpgsqlDbType.Text));
                    command.Parameters.Add(new NpgsqlParameter("@age", NpgsqlDbType.Integer));
                    command.Parameters.Add(new NpgsqlParameter("@details", NpgsqlDbType.Text));
                    command.Parameters.Add(new NpgsqlParameter("@session_start_timestamp", NpgsqlDbType.Timestamp));
                    command.Parameters.Add(new NpgsqlParameter("@session_end_timestamp", NpgsqlDbType.Timestamp));

                    command.Parameters[0].Value = Common.ReplaceNullToDbNull(subjectInfo.SessionId);
                    command.Parameters[1].Value = Common.ReplaceNullToDbNull(subjectInfo.Name);
                    command.Parameters[2].Value = Common.ReplaceNullToDbNull(subjectInfo.Age);
                    command.Parameters[3].Value = Common.ReplaceNullToDbNull(subjectInfo.Details);
                    command.Parameters[4].Value = Common.ReplaceNullToDbNull(subjectInfo.SessionStartTimestamp);
                    command.Parameters[5].Value = Common.ReplaceNullToDbNull(subjectInfo.SessionEndTimeStamp);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void BinaryInsertGazePoints(IEnumerable<GazePoint> gazePoints, string sessionId, int? subjectInfoId, DateTime sampleTime) {
            if (gazePoints == null) {
                throw new ArgumentNullException(nameof(gazePoints));
            }

            using (var connection = Common.CreateConnection(_connectionString)) {
                var partitionName = Common.GetPartitionName(sampleTime);

                string copyFromCommand = $"COPY {Constants.GazePointTableName}_{partitionName} ({string.Join(",", _gazePointsColumn)}) FROM STDIN (FORMAT BINARY)";

                using (NpgsqlBinaryImporter npgsqlBinaryImporter = connection.BeginBinaryImport(copyFromCommand)) {
                    foreach (var gazePoint in gazePoints) {
                        npgsqlBinaryImporter.StartRow();
                        npgsqlBinaryImporter.Write(gazePoint.X, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(gazePoint.Y, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(gazePoint.Timestamp, NpgsqlDbType.Bigint);
                        npgsqlBinaryImporter.Write(sessionId, NpgsqlDbType.Uuid);
                        npgsqlBinaryImporter.Write(subjectInfoId, NpgsqlDbType.Integer);
                        npgsqlBinaryImporter.Write(sampleTime, NpgsqlDbType.Timestamp);
                    }
                }
            }
        }

        public void BinaryInsertSaccades(IEnumerable<Saccade> saccades, string sessionId, int? subjectInfoId, DateTime sampleTime) {
            if (saccades == null) {
                throw new ArgumentNullException(nameof(saccades));
            }

            using (var connection = Common.CreateConnection(_connectionString)) {
                var partitionName = Common.GetPartitionName(sampleTime);

                string copyFromCommand = $"COPY {Constants.SaccadeTableName}_{partitionName} ({string.Join(",", _saccadeColumns)}) FROM STDIN (FORMAT BINARY)";

                using (NpgsqlBinaryImporter npgsqlBinaryImporter = connection.BeginBinaryImport(copyFromCommand)) {
                    foreach (var saccade in saccades) {
                        npgsqlBinaryImporter.StartRow();
                        npgsqlBinaryImporter.Write(saccade.Direction, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(saccade.Amplitude, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(saccade.Velocity, NpgsqlDbType.Double);
                        npgsqlBinaryImporter.Write(saccade.StartTimeStamp, NpgsqlDbType.Bigint);
                        npgsqlBinaryImporter.Write(saccade.EndTimeStamp, NpgsqlDbType.Bigint);
                        npgsqlBinaryImporter.Write(sessionId, NpgsqlDbType.Uuid);
                        npgsqlBinaryImporter.Write(subjectInfoId, NpgsqlDbType.Integer);
                        npgsqlBinaryImporter.Write(sampleTime, NpgsqlDbType.Timestamp);
                    }
                }
            }
        }

        public int DeleteSubjectInfo(string sessionId)
        {
            int result;

            using (var connection = Common.CreateConnection(_connectionString))
            {
                string commandText = $"DELETE FROM {Constants.SubjectInfoTableName} "
                                     + "WHERE session_id=@session_id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@session_id", NpgsqlDbType.Uuid, sessionId);
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }

        public List<DbGazePoint> SelectGazePoints(int subjectInfoId)
        {
            var gazePoints = new List<DbGazePoint>();

            using (var connection = Common.CreateConnection(_connectionString))
            {
                string commandText = $"SELECT * FROM {Constants.GazePointTableName} " +
                                     "WHERE subject_info_id = @subject_info_id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@subject_info_id", NpgsqlDbType.Integer, subjectInfoId);

                    try
                    {
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                gazePoints.Add(new DbGazePoint
                                {
                                    X = Convert.ToDouble(reader["x"]),
                                    Y = Convert.ToDouble(reader["y"]),
                                    SampleTime = Convert.ToDateTime(reader["sample_time"]),
                                    Timestamp = Convert.ToInt64(reader["timestamp"]),
                                    SessionId = Convert.ToString(reader["session_id"]),
                                    SubjectInfoId = Convert.ToInt32(reader["subject_info_id"])
                                });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //_logger.Error(e);
                    }
                }
            }

            return gazePoints;
        }

        public List<DbSaccade> SelectSaccades(int subjectInfoId)
        {
            var saccades = new List<DbSaccade>();

            using (var connection = Common.CreateConnection(_connectionString))
            {
                string commandText = $"SELECT * FROM {Constants.SaccadeTableName} " +
                                     "WHERE subject_info_id = @subject_info_id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@subject_info_id", NpgsqlDbType.Integer, subjectInfoId);

                    try
                    {
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                saccades.Add(new DbSaccade
                                {
                                    Direction = Convert.ToDouble(reader["direction"]),
                                    Amplitude = Convert.ToDouble(reader["amplitude"]),
                                    Velocity = Convert.ToDouble(reader["velocity"]),
                                    SampleTime = Convert.ToDateTime(reader["sample_time"]),
                                    SessionId = Convert.ToString(reader["session_id"]),
                                    SubjectInfoId = Convert.ToInt32(reader["subject_info_id"]),
                                    StartTimeStamp = Convert.ToInt64(reader["start_timestamp"]),
                                    EndTimeStamp = Convert.ToInt64(reader["end_timestamp"])
                                });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //_logger.Error(e);
                    }
                }
            }

            return saccades;
        }

        public int DeleteGazePoints(int subjectInfoId)
        {
            int result;

            using (var connection = Common.CreateConnection(_connectionString))
            {
                string commandText = $"DELETE FROM {Constants.GazePointTableName} "
                                     + "WHERE subject_info_id=@subject_info_id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@subject_info_id", NpgsqlDbType.Integer, subjectInfoId);
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }

        public int DeleteSaccades(int subjectInfoId)
        {
            int result;

            using (var connection = Common.CreateConnection(_connectionString))
            {
                string commandText = $"DELETE FROM {Constants.SaccadeTableName} "
                                     + "WHERE subject_info_id=@subject_info_id";
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@subject_info_id", NpgsqlDbType.Integer, subjectInfoId);
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }
    }
}
