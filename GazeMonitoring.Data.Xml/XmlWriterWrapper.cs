using System;
using System.IO;
using System.Xml;

namespace GazeMonitoring.Data.Xml {
    public class XmlWriterWrapper : IDisposable {
        private readonly FileStream _fileStream;
        public XmlWriter XmlWriter { get; }

        public XmlWriterWrapper(FileStream fileStream, XmlWriter xmlWriter) {
            _fileStream = fileStream;
            XmlWriter = xmlWriter;
        }
        public void Dispose() {
            XmlWriter.WriteEndElement();
            XmlWriter.WriteEndElement();

            XmlWriter.WriteEndDocument();

            XmlWriter.Dispose();
            _fileStream.Dispose();
        }
    }
}
