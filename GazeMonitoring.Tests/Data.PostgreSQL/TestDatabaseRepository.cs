using System;
using System.Collections.Generic;
using System.Data.Common;
using GazeMonitoring.Data.PostgreSQL;
using Npgsql;
using NpgsqlTypes;
using static GazeMonitoring.Data.PostgreSQL.Common;

namespace GazeMonitoring.Tests.Data.PostgreSQL {
    public class TestDatabaseRepository {
        private readonly string _connectionString = "";

        public int DeleteSubjectInfo(string sessionId) {
            int result;

            using (var connection = CreateConnection(_connectionString)) {
                string commandText = $"DELETE FROM {Constants.SubjectInfoTableName}"
                                     + "WHERE session_id = @session_id";
                using (var command = new NpgsqlCommand(commandText, connection)) {
                    command.Parameters.AddWithValue("@session_id", NpgsqlDbType.Uuid, sessionId);
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }

        public List<DbGazePoint> SelectGazePoints(int subjectInfoId) {
            var gazePoints = new List<DbGazePoint>();

            using (var connection = CreateConnection(_connectionString)) {
                string commandText = $"SELECT * FROM {Constants.GazePointTableName} " +
                                     "WHERE subject_info_id = @subject_info_id";
                using (var command = new NpgsqlCommand(commandText, connection)) {
                    command.Parameters.AddWithValue("@subject_info_id", NpgsqlDbType.Integer, subjectInfoId);

                    try {
                        using (DbDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                gazePoints.Add(new DbGazePoint {
                                    X = Convert.ToDouble(reader["x"]),
                                    Y = Convert.ToDouble(reader["y"]),
                                    SampleTime = Convert.ToDateTime(reader["sample_time"]),
                                    Timestamp = Convert.ToInt64(reader["timestamp"]),
                                    SessionId = Convert.ToString(reader["sessiond_id"]),
                                    SubjectInfoId = Convert.ToInt32(reader["sessiond_id"])
                                });
                            }
                        }
                    } catch (Exception e) {
                        //_logger.Error(e);
                    }
                }
            }

            return gazePoints;
        }

        public List<DbSaccade> SelectSaccades(int subjectInfoId) {
            var saccades = new List<DbSaccade>();

            using (var connection = CreateConnection(_connectionString)) {
                string commandText = $"SELECT * FROM {Constants.GazePointTableName} " +
                                     "WHERE subject_info_id = @subject_info_id";
                using (var command = new NpgsqlCommand(commandText, connection)) {
                    command.Parameters.AddWithValue("@subject_info_id", NpgsqlDbType.Integer, subjectInfoId);

                    try {
                        using (DbDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                saccades.Add(new DbSaccade {
                                    Direction = Convert.ToDouble(reader["direction"]),
                                    Amplitude = Convert.ToDouble(reader["amplitude"]),
                                    Velocity = Convert.ToDouble(reader["velocity"]),
                                    SampleTime = Convert.ToDateTime(reader["sample_time"]),
                                    SessionId = Convert.ToString(reader["sessiond_id"]),
                                    SubjectInfoId = Convert.ToInt32(reader["sessiond_id"]),
                                    StartTimeStamp = Convert.ToInt64(reader["start_timestamp"]),
                                    EndTimeStamp = Convert.ToInt64(reader["end_timestamp"])
                                });
                            }
                        }
                    } catch (Exception e) {
                        //_logger.Error(e);
                    }
                }
            }

            return saccades;
        }

        public int DeleteGazePoints(int subjectInfoId) {
            int result;

            using (var connection = CreateConnection(_connectionString)) {
                string commandText = $"DELETE FROM {Constants.GazePointTableName}"
                                     + "WHERE subject_info_id = @subject_info_id";
                using (var command = new NpgsqlCommand(commandText, connection)) {
                    command.Parameters.AddWithValue("@subject_info_id", NpgsqlDbType.Integer, subjectInfoId);
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }

        public int DeleteSaccades(int subjectInfoId) {
            int result;

            using (var connection = CreateConnection(_connectionString)) {
                string commandText = $"DELETE FROM {Constants.SaccadeTableName}"
                                     + "WHERE subject_info_id = @subject_info_id";
                using (var command = new NpgsqlCommand(commandText, connection)) {
                    command.Parameters.AddWithValue("@subject_info_id", NpgsqlDbType.Integer, subjectInfoId);
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }
    }
}
