using GazeMonitoring.Model;

namespace GazeMonitoring.Messaging.Messages
{
    public class ShowMonitoringConfigurationDetailsMessage : IMessage
    {
        public ShowMonitoringConfigurationDetailsMessage(MonitoringConfiguration monitoringConfiguration)
        {
            MonitoringConfiguration = monitoringConfiguration;
        }

        public MonitoringConfiguration MonitoringConfiguration { get; }
    }
}
