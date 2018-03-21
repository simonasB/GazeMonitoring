using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.Csv {
    public class CsvFileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            return $"{fileName}.csv";
        }
    }
}
