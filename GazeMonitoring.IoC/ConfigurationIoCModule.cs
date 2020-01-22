using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace GazeMonitoring.IoC
{
    public class ConfigurationIoCModule : IoCModule
    {
        private readonly IConfiguration _configuration;

        public ConfigurationIoCModule(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            _configuration = configuration;
        }

        public void Load(IoContainerBuilder builder)
        {
            var modules = new List<IoCModule>();

            foreach (var configurationSection in GetOrderedSubsections(_configuration, "modules"))
            {
                var typeName = configurationSection["type"];

                if (typeName == null)
                {
                    throw new GazeMonitoringIoCException("Modules must have 'type' field specified");
                }

                try
                {
                    var type = Type.GetType(typeName);

                    var module = (IoCModule) Activator.CreateInstance(type);
                    modules.Add(module);
                }
                catch (Exception ex)
                {
                    throw new GazeMonitoringIoCException("Invalid type specified.", ex);
                }
            }

            modules.ForEach(o => o.Load(builder));
        }

        private static IEnumerable<IConfigurationSection> GetOrderedSubsections(IConfiguration configuration, string key)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var configurationSection = configuration.GetSection(key);
            foreach (var section in configurationSection.GetChildren())
            {
                if (int.TryParse(section.Key, out _))
                {
                    yield return section;
                }
                else
                {
                    throw new GazeMonitoringIoCException("Section key must be integer value.");
                }
            }
        }
    }
}
