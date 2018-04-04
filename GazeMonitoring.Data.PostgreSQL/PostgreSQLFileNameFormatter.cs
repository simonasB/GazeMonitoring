using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.PostgreSQL {
    public class PostgreSQLFileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            return fileName.DataStream.Contains("Saccades") ? Constants.SaccadesTempCsvFileName : Constants.GazePointsTempCsvFileName;
        }
    }
}
