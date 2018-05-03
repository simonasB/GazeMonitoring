using System;

namespace GazeMonitoring.Data {
    public class StandardFileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            if (fileName == null) {
                throw new ArgumentNullException(nameof(fileName));
            }

            return fileName.ToString();
        }
    }
}
