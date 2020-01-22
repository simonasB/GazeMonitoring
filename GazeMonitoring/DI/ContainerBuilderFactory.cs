using GazeMonitoring.IoC;
using GazeMonitoring.IoC.Autofac;

namespace GazeMonitoring.DI
{
    public static class ContainerBuilderFactory
    {
        public static IoContainerBuilder Create()
        {
            return new AutofacContainerBuilder();
        }
    }
}
