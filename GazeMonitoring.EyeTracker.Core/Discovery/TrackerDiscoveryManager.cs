using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Autofac;

namespace GazeMonitoring.EyeTracker.Core.Discovery {
    public class TrackerDiscoveryManager {
        public void Discover(ContainerBuilder container) {
            // Force all referenced assemblies to be loaded into the app domain
            new DirectoryCatalog(".");
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IDiscoverable).IsAssignableFrom(p) && !p.IsInterface);
            foreach (var type in types) {
                var discoverable = (IDiscoverable)Activator.CreateInstance(type);
                var discoveryResult = discoverable.Discover(container);

                if (discoveryResult.IsActive) {
                    return;
                }
            }
        }
    }
}
