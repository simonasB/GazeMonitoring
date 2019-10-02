namespace GazeMonitoring.Base
{
    public interface IAppLocalContext
    {
        int? CurrentConfigurationId { get; set; }
    }

    public class AppLocalContext : IAppLocalContext
    {
        public int? CurrentConfigurationId { get; set; }
    }
}
