using GazeMonitoring.Model;

namespace GazeMonitoring.Messaging
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
