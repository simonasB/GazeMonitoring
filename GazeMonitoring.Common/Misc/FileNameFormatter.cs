using GazeMonitoring.Model;

namespace GazeMonitoring.Common.Misc {
    public class FileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            return fileName.ToString();
        }
    }
}
