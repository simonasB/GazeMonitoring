using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Xml {
    public class XmlFileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            return $"{fileName}.xml";
        }
    }
}
