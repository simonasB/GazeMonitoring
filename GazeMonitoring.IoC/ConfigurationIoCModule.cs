using Microsoft.Extensions.Configuration;

namespace GazeMonitoring.IoC
{
    public class ConfigurationIoCModule : IoCModule
    {
        private readonly IConfiguration _configurationRoot;

        public ConfigurationIoCModule(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public void Load(IoContainerBuilder builder)
        {
            
            throw new System.NotImplementedException();
        }
    }
}
