using System.Linq;
using GazeMonitoring.DataAccess;

namespace GazeMonitoring.Base
{
    public interface IAppLocalContextManager
    {
        void Save(AppLocalContext appLocalContext);

        AppLocalContext Get();
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
    }
}
