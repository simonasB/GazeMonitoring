namespace GazeMonitoring.Data {
    public class FileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            return fileName.ToString();
        }
    }
}
