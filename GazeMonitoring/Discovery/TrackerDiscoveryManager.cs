﻿using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using GazeMonitoring.EyeTracker.Core.Discovery;
using GazeMonitoring.IoC;

namespace GazeMonitoring.Discovery {
    public class TrackerDiscoveryManager {
        public void Discover(IoContainerBuilder container) {
            // Force all referenced assemblies to be loaded into the app domain
            new DirectoryCatalog(".");

            /*var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", "eyetracker", "custom", "CustomMonitoring.dll");

            if (File.Exists(path))
            {
                AssemblyName an = AssemblyName.GetAssemblyName(path);
                Assembly.Load(an);
            }
            else
            {
                var tobbiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", "eyetracker", "tobii", "TobiiCoreMonitoring.dll");
                AssemblyName an = AssemblyName.GetAssemblyName(tobbiPath);
                Assembly.Load(an);
            }*/

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
