namespace GazeMonitoring.Model
{
    public interface IMonitoringContext
    {
        DataStream DataStream { get; set; }
        SubjectInfo SubjectInfo { get; set; }

        string DataFilesPath { get; set; }

        MonitoringConfiguration MonitoringConfiguration { get; set; }

        bool IsScreenRecorded { get; set; }
        bool IsReportGenerated { get; set; }
        bool IsAnonymous { get; set; }
    }

    public class MonitoringContext : IMonitoringContext
    {
        public DataStream DataStream { get; set; }
        public SubjectInfo SubjectInfo { get; set; }
        public string DataFilesPath { get; set; }
        public MonitoringConfiguration MonitoringConfiguration { get; set; }
        public bool IsScreenRecorded { get; set; }
        public bool IsReportGenerated { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
