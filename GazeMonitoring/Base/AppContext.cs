namespace GazeMonitoring.Base
{
    public interface IAppLocalContext
    {
        string ScreenConfigurationId { get; set; }
        int? MonitoringConfigurationId { get; set; }
    }

    public class AppLocalContext : IAppLocalContext
    {
        public string ScreenConfigurationId { get; set; }
        public int? MonitoringConfigurationId { get; set; }
    }
}
