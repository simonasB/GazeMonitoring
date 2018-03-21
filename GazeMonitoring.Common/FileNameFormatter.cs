using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Common {
    public class FileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            return fileName.ToString();
        }
    }
}
