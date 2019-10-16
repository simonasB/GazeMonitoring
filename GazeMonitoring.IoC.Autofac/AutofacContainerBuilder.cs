using System;
using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;

namespace GazeMonitoring.IoC.Autofac
{
    public class AutofacContainerBuilder : IoContainerBuilder
    {
        private readonly ContainerBuilder _containerBuilder;

        public AutofacContainerBuilder()
        {
            _containerBuilder = new ContainerBuilder();
        }

        public IoContainerBuilder RegisterSingleton<T>(T instance) where T : class
        {
            _containerBuilder.RegisterInstance(instance).SingleInstance();
            return this;
        }

        public IoContainerBuilder Register<T>() where T : class
        {
            _containerBuilder.RegisterType<T>();
            return this;
        }

        public IoContainerBuilder Register<T>(Scope scope) where T : class
        {
            switch (scope)
            {
                case Scope.Singleton:
                    _containerBuilder.RegisterType<T>().SingleInstance();
                    break;
                case Scope.Transient:
                    _containerBuilder.RegisterType<T>().InstancePerDependency();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("scope");
            }

            return this;
        }

        public IoContainerBuilder Register<TInterface, TImplementation>() where TInterface : class where TImplementation : class, TInterface
        {
            _containerBuilder.RegisterType<TImplementation>().As<TInterface>();
            return this;
        }

        public IoContainerBuilder Register<TInterface, TImplementation>(Scope scope) where TInterface : class where TImplementation : class, TInterface
        {
            switch (scope)
            {
                case Scope.Singleton:
                    _containerBuilder.RegisterType<TImplementation>().As<TInterface>().SingleInstance();
                    break;
                case Scope.Transient:
                    _containerBuilder.RegisterType<TImplementation>().As<TInterface>().InstancePerDependency();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("scope");
            }
            return this;
        }

        public IoContainerBuilder Register<T>(Func<IoContainer, T> factory) where T : class
        {
            _containerBuilder.Register(context => factory(new AutofacContainer(context))).InstancePerDependency();
            return this;
        }

        public IoContainerBuilder Register<T>(Func<IoContainer, T> factory, Scope scope) where T : class
        {
            switch (scope)
            {
                case Scope.Singleton:
                    _containerBuilder.Register(context => factory(new AutofacContainer(context))).SingleInstance();
                    break;
                case Scope.Transient:
                    _containerBuilder.Register(context => factory(new AutofacContainer(context))).InstancePerDependency();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("scope");
            }
            return this;
        }

        public IoContainerBuilder RegisterModule<T>() where T : IoCModule, new()
        {
            var module = new T();
            module.Load(this);
            return this;
        }

        public IoContainerBuilder RegisterModule<T>(T instance) where T : IoCModule, new()
        {
            instance.Load(this);
            return this;
        }

        public IoContainerBuilder RegisterModule(IConfigurationRoot configurationRoot)
        {
            var module = new ConfigurationModule(configurationRoot);
            _containerBuilder.RegisterModule(module);
            return this;
        }

        public IoContainer Build()
        {
            return new AutofacContainer(_containerBuilder.Build());
        }
    }
}
