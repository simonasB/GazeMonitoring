using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Writers
{
    public class NullGazeDataWriterFactory : IGazeDataWriterFactory
    {
        public IGazeDataWriter Create(IMonitoringContext monitoringContext)
        {
            throw new NotImplementedException();
        }
    }
}
