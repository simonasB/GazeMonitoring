using System;
using Autofac;

namespace GazeMonitoring.IoC.Autofac
{
    public class AutofacContainer : IoContainer
    {
        private readonly IComponentContext _container;

        public AutofacContainer(IComponentContext container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }

        public bool IsRegistered<T>()
        {
            return _container.IsRegistered<T>();
        }

        public T GetInstance<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
