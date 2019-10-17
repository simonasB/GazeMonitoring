using System;
using System.Runtime.Serialization;

namespace GazeMonitoring.IoC
{
    public class GazeMonitoringIoCException : Exception
    {
        public GazeMonitoringIoCException()
        {
        }

        public GazeMonitoringIoCException(string message) : base(message)
        {
        }

        public GazeMonitoringIoCException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GazeMonitoringIoCException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
