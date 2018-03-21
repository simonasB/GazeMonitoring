using GazeMonitoring.Common;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.Xml {
    public class XmlFileNameFormatter : IFileNameFormatter {
        public string Format(FileName fileName) {
            return $"{fileName}.xml";
        }
    }
}
