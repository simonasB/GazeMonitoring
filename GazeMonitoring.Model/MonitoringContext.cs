namespace GazeMonitoring.Model
{
    public interface IMonitoringContext
    {
        DataStream DataStream { get; set; }
        SubjectInfo SubjectInfo { get; set; }

        string DataFilesPath { get; set; }

        MonitoringConfiguration MonitoringConfiguration { get; set; }
    }

    public class MonitoringContext : IMonitoringContext
    {
        public DataStream DataStream { get; set; }
        public SubjectInfo SubjectInfo { get; set; }
        public string DataFilesPath { get; set; }
        public MonitoringConfiguration MonitoringConfiguration { get; set; }
    }
}
