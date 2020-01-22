namespace GazeMonitoring.Base
{
    public class AppLocalContext
    {
        public int Id { get; set; }

        public string ScreenConfigurationId { get; set; }

        public int? MonitoringConfigurationId { get; set; }

        public int? ProfileId { get; set; }

        public string DataFilesPath { get; set; }
    }
}
