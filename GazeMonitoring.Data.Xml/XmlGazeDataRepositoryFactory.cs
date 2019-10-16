using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Xml
{
    public class XmlGazeDataRepositoryFactory : IGazeDataRepositoryFactory
    {
        private readonly IXmlWritersFactory _xmlWritersFactory;

        public XmlGazeDataRepositoryFactory(IXmlWritersFactory xmlWritersFactory)
        {
            _xmlWritersFactory = xmlWritersFactory;
        }

        public IGazeDataRepository Create(IMonitoringContext monitoringContext)
        {
            return new XmlGazeDataRepository(_xmlWritersFactory.GetXmlWriters(monitoringContext));
        }
    }
}
