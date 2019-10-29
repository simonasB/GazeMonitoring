using System.Linq;
using GazeMonitoring.DataAccess;

namespace GazeMonitoring.Base
{
    public interface IAppLocalContextManager
    {
        void Save(AppLocalContext appLocalContext);

        AppLocalContext Get();

        // TODO: Might extracted to extensions methods
        void SetScreenConfigurationId(string screenConfigurationId);
        void SetMonitoringConfigurationId(int monitoringConfigurationId);
    }

    public class AppLocalContextManager : IAppLocalContextManager
    {
        private readonly IConfigurationRepository _configurationRepository;

        public AppLocalContextManager(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public void Save(AppLocalContext appLocalContext)
        {
            _configurationRepository.Save(appLocalContext);
        }

        public AppLocalContext Get()
        {
            var appLocalContext = _configurationRepository.Search<AppLocalContext>().FirstOrDefault();

            return appLocalContext;
        }

        public void SetScreenConfigurationId(string screenConfigurationId)
        {
            var appLocalContext = Get();
            appLocalContext.ScreenConfigurationId = screenConfigurationId;
            _configurationRepository.Update(appLocalContext);
        }

        public void SetMonitoringConfigurationId(int monitoringConfigurationId)
        {
            var appLocalContext = Get();
            appLocalContext.MonitoringConfigurationId = monitoringConfigurationId;
            _configurationRepository.Update(appLocalContext);
        }
    }
}
