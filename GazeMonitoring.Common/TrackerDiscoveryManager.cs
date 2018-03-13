using System;
using System.Linq;
using Autofac;

namespace GazeMonitoring.Common {
    public class TrackerDiscoveryManager {
        public void Discover(ContainerBuilder container) {
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
