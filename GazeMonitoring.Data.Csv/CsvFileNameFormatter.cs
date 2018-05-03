using System;

namespace GazeMonitoring.Data.Csv {
    public class CsvFileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            if (fileName == null) {
                throw new ArgumentNullException(nameof(fileName));
            }

            return $"{fileName}.csv";
        }
    }
}
