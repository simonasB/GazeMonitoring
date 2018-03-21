using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.Xml {
    public class XmlGazeDataRepository : IGazeDataRepository, IDisposable {
        private readonly Dictionary<Type, XmlWriterWrapper> _xmlWriterWrappers;
        private readonly Dictionary<Type, XmlSerializer> _xmlSerializers;

        public XmlGazeDataRepository(XmlWritersFactory xmlWritersFactory, DataStream dataStream) {
            _xmlWriterWrappers = xmlWritersFactory.GetXmlWriters(dataStream);
            _xmlSerializers = _xmlWriterWrappers.ToDictionary(kvp => kvp.Key, kvp => new XmlSerializer(kvp.Key, new XmlRootAttribute(kvp.Key.Name)));
        }

        public void SaveMany<TEntity>(IEnumerable<TEntity> entities) {
            throw new System.NotImplementedException();
        }

        public void SaveOne<TEntity>(TEntity entity) {
            Write(entity);
        }

        public void Dispose() {
            foreach (var xmlWriterWrapper in _xmlWriterWrappers) {
                xmlWriterWrapper.Value?.Dispose();
            }
        }

        private void Write<T>(T entity) {
            var type = typeof(T);
            // Omitting all xsi and xsd namespaces
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            _xmlSerializers[type].Serialize(_xmlWriterWrappers[type].XmlWriter, entity, ns);
        }
    }
}
