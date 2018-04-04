using System;
using Npgsql;

namespace GazeMonitoring.Data.PostgreSQL {
    public class Common {
        public static NpgsqlConnection CreateConnection(string connectionString) {
            NpgsqlConnection conn = null;
            try {
                conn = new NpgsqlConnection(connectionString);
                conn.Open();
            } catch (Exception exp) {
                Console.WriteLine(exp);
                //_logger.Error($"Failed to connect to PostgreSQL: {exp.Message}");
            }
            return conn;
        }

        public static string GetPartitionName(DateTime date) {
            string name = date.ToString("yyyMMdd");
            return name;
        }

        public static object ReplaceNullToDbNull<T>(T value) {
            var result = value == null ? (object) DBNull.Value : value;
            return result;
        }
    }
}
