namespace GazeMonitoring.Data.CustomExample {
    public class ExampleFileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            return $"{fileName}.example";
        }
    }
}
