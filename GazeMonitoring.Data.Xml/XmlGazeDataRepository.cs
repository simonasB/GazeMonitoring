﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Xml {
    public class XmlGazeDataRepository : IGazeDataRepository {
        private readonly Dictionary<Type, XmlWriterWrapper> _xmlWriterWrappers;
        private readonly Dictionary<Type, XmlSerializer> _xmlSerializers;

        public XmlGazeDataRepository(Dictionary<Type, XmlWriterWrapper> xmlWriterWrappers) {
            if (xmlWriterWrappers == null) {
                throw new ArgumentNullException(nameof(xmlWriterWrappers));
            }

            _xmlWriterWrappers = xmlWriterWrappers;
            _xmlSerializers = _xmlWriterWrappers.ToDictionary(kvp => kvp.Key, kvp => new XmlSerializer(kvp.Key, new XmlRootAttribute(kvp.Key.Name)));
        }

        public void SaveGazePoint(GazePoint gazePoint) {
            if (gazePoint == null) {
                throw new ArgumentNullException(nameof(gazePoint));
            }

            Write(gazePoint);
        }

        public void SaveSaccade(Saccade saccade) {
            if (saccade == null) {
                throw new ArgumentNullException(nameof(saccade));
            }

            Write(saccade);
        }

        public void SaveFixationPoint(FixationPoint fixationPoint) {
            if (fixationPoint == null)
            {
                throw new ArgumentNullException(nameof(fixationPoint));
            }

            Write(fixationPoint);
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
