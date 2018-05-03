using System;

namespace GazeMonitoring.Data.PostgreSQL {
    public class PostgreSQLFileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            if (fileName == null) {
                throw new ArgumentNullException(nameof(fileName));
            }

            return fileName.DataStream.Contains("Saccades") ? Constants.SaccadesTempCsvFileName : Constants.GazePointsTempCsvFileName;
        }
    }
}
