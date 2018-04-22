using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Xml {
    public class XmlWritersFactory {
        private readonly IFileNameFormatter _fileNameFormatter;
        private readonly SubjectInfo _subjectInfo;

        public XmlWritersFactory(IFileNameFormatter fileNameFormatter, SubjectInfo subjectInfo)
        {
            _fileNameFormatter = fileNameFormatter;
            _subjectInfo = subjectInfo;
        }

        public Dictionary<Type, XmlWriterWrapper> GetXmlWriters(DataStream dataStream)
        {
            var csvWriters = new Dictionary<Type, XmlWriterWrapper>();

            switch (dataStream)
            {
                case DataStream.UnfilteredGaze:
                    csvWriters.Add(typeof(GazePoint), CreateXmlWriter<GazePoint>(dataStream.ToString()));
                    break;
                case DataStream.LightlyFilteredGaze:
                    csvWriters.Add(typeof(GazePoint), CreateXmlWriter<GazePoint>(dataStream.ToString()));
                    break;
                case DataStream.SensitiveFixation:
                    csvWriters.Add(typeof(GazePoint), CreateXmlWriter<GazePoint>(dataStream.ToString()));
                    csvWriters.Add(typeof(Saccade), CreateXmlWriter<Saccade>($"{dataStream}_Saccades"));
                    break;
                case DataStream.SlowFixation:
                    csvWriters.Add(typeof(GazePoint), CreateXmlWriter<GazePoint>(dataStream.ToString()));
                    csvWriters.Add(typeof(Saccade), CreateXmlWriter<Saccade>($"{dataStream}_Saccades"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }

            return csvWriters;
        }

        private XmlWriterWrapper CreateXmlWriter<T>(string dataStream)
        {
            var fileName = new FileName { DataStream = dataStream };
            var fileStream = File.Create(Path.Combine(Directory.GetCurrentDirectory(), _fileNameFormatter.Format(fileName)));
            var xmlWriter = XmlWriter.Create(fileStream); ;

            Initialize<T>(xmlWriter);
            return new XmlWriterWrapper(fileStream, xmlWriter);
        }

        private void Initialize<T>(XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("GazeData");

            var serializer = new XmlSerializer(_subjectInfo.GetType(), new XmlRootAttribute(nameof(SubjectInfo)));
            serializer.Serialize(writer, _subjectInfo);
            writer.WriteStartElement($"{typeof(T).Name}s");
        }
    }
}
