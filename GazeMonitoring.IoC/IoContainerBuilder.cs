using System;
using Microsoft.Extensions.Configuration;

namespace GazeMonitoring.IoC
{
    public interface IoContainerBuilder
    {
        IoContainerBuilder RegisterSingleton<T>(T instance) where T : class;
        IoContainerBuilder Register<T>() where T : class;
        IoContainerBuilder Register<T>(Scope scope) where T : class;

        IoContainerBuilder Register<TInterface, TImplementation>() where TInterface : class where TImplementation : class, TInterface;
        IoContainerBuilder Register<TInterface, TImplementation>(Scope scope) where TInterface : class where TImplementation : class, TInterface;

        IoContainerBuilder Register<T>(Func<IoContainer, T> factory) where T : class;
        IoContainerBuilder Register<T>(Func<IoContainer, T> factory, Scope scope) where T : class;

        IoContainerBuilder RegisterModule<T>() where T : IoCModule, new();
        IoContainerBuilder RegisterModule<T>(T instance) where T : IoCModule, new();

        IoContainerBuilder RegisterModule(IConfigurationRoot configurationRoot);

        IoContainer Build();
    }
}
