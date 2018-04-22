using GazeMonitoring.Common.Misc;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.PostgreSQL {
    public class PostgreSQLFileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            return fileName.DataStream.Contains("Saccades") ? Constants.SaccadesTempCsvFileName : Constants.GazePointsTempCsvFileName;
        }
    }
}
