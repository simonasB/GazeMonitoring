using System.Linq;
using GazeMonitoring.DataAccess;

namespace GazeMonitoring.Base
{
    public interface IAppLocalContextManager
    {
        void Save(AppLocalContext appLocalContext);

        void Update(AppLocalContext appLocalContext);

        AppLocalContext Get();
    }

    public static class AppLocalContextManagerExtensions
    {
        public static void SetScreenConfigurationId(this IAppLocalContextManager appLocalContextManager, string screenConfigurationId)
        {
            var appLocalContext = appLocalContextManager.Get();
            appLocalContext.ScreenConfigurationId = screenConfigurationId;
            appLocalContextManager.Update(appLocalContext);
        }

        public static void SetMonitoringConfigurationId(this IAppLocalContextManager appLocalContextManager, int? monitoringConfigurationId)
        {
            var appLocalContext = appLocalContextManager.Get();
            appLocalContext.MonitoringConfigurationId = monitoringConfigurationId;
            appLocalContextManager.Update(appLocalContext);
        }

        public static void SetDataFilesPath(this IAppLocalContextManager appLocalContextManager, string dataFilesPath)
        {
            var appLocalContext = appLocalContextManager.Get();
            appLocalContext.DataFilesPath = dataFilesPath;
            appLocalContextManager.Update(appLocalContext);
        }
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

        public void Update(AppLocalContext appLocalContext)
        {
            _configurationRepository.Update(appLocalContext);
        }

        public AppLocalContext Get()
        {
            var appLocalContext = _configurationRepository.Search<AppLocalContext>().FirstOrDefault();

            return appLocalContext;
        }
    }
}
