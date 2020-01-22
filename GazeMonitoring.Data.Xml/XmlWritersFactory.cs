using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Xml {
    public class XmlWritersFactory : IXmlWritersFactory {
        private readonly IFileNameFormatter _fileNameFormatter;

        public XmlWritersFactory(IFileNameFormatter fileNameFormatter)
        {
            if (fileNameFormatter == null) {
                throw new ArgumentNullException(nameof(fileNameFormatter));
            }

            _fileNameFormatter = fileNameFormatter;
        }

        public Dictionary<Type, XmlWriterWrapper> GetXmlWriters(IMonitoringContext monitoringContext)
        {
            var csvWriters = new Dictionary<Type, XmlWriterWrapper>();
            var dataStream = monitoringContext.DataStream;
            var subjectInfo = monitoringContext.SubjectInfo;

            switch (dataStream)
            {
                case DataStream.UnfilteredGaze:
                    csvWriters.Add(typeof(GazePoint), CreateXmlWriter<GazePoint>(dataStream.ToString(), subjectInfo, monitoringContext.DataFilesPath));
                    break;
                case DataStream.LightlyFilteredGaze:
                    csvWriters.Add(typeof(GazePoint), CreateXmlWriter<GazePoint>(dataStream.ToString(), subjectInfo, monitoringContext.DataFilesPath));
                    break;
                case DataStream.SensitiveFixation:
                    csvWriters.Add(typeof(GazePoint), CreateXmlWriter<GazePoint>(dataStream.ToString(), subjectInfo, monitoringContext.DataFilesPath));
                    csvWriters.Add(typeof(Saccade), CreateXmlWriter<Saccade>($"{dataStream}_Saccades", subjectInfo, monitoringContext.DataFilesPath));
                    break;
                case DataStream.SlowFixation:
                    csvWriters.Add(typeof(GazePoint), CreateXmlWriter<GazePoint>(dataStream.ToString(), subjectInfo, monitoringContext.DataFilesPath));
                    csvWriters.Add(typeof(Saccade), CreateXmlWriter<Saccade>($"{dataStream}_Saccades", subjectInfo, monitoringContext.DataFilesPath));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStream), dataStream, null);
            }

            return csvWriters;
        }

        private XmlWriterWrapper CreateXmlWriter<T>(string dataStream, SubjectInfo subjectInfo, string dataFilesPath)
        {
            var fileName = new FileName { DataStream = dataStream, DateTime = DateTime.Now };

            const string xmlFolderName = "data_xml";
            var xmlFolderPath = Path.Combine(dataFilesPath, xmlFolderName);

            if (!Directory.Exists(xmlFolderPath)) {
                Directory.CreateDirectory(xmlFolderPath);
            }

            var fileStream = File.Create(Path.Combine(xmlFolderPath, _fileNameFormatter.Format(fileName)));
            var xmlWriter = XmlWriter.Create(fileStream);

            Initialize<T>(xmlWriter, subjectInfo);
            return new XmlWriterWrapper(fileStream, xmlWriter);
        }

        private void Initialize<T>(XmlWriter writer, SubjectInfo subjectInfo)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement($"{typeof(T).Name}sData");

            var serializer = new XmlSerializer(subjectInfo.GetType(), new XmlRootAttribute(nameof(SubjectInfo)));
            serializer.Serialize(writer, subjectInfo);
            writer.WriteStartElement($"{typeof(T).Name}s");
        }
    }
}
