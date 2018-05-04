using System;
using System.Collections.Generic;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Xml {
    public interface IXmlWritersFactory {
        Dictionary<Type, XmlWriterWrapper> GetXmlWriters(DataStream dataStream);
    }
}