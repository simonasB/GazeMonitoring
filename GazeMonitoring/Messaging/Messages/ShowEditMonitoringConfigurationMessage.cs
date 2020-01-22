using GazeMonitoring.Model;

namespace GazeMonitoring.Messaging.Messages
{
    public class ShowEditMonitoringConfigurationMessage : IMessage
    {
        public ShowEditMonitoringConfigurationMessage(MonitoringConfiguration monitoringConfiguration)
        {
            MonitoringConfiguration = monitoringConfiguration;
        }

        public MonitoringConfiguration MonitoringConfiguration { get; }
    }
}
