using GazeMonitoring.IoC.Autofac;

namespace GazeMonitoring.IoC
{
    public static class ContainerBuilderFactory
    {
        public static IoContainerBuilder Create()
        {
            return new AutofacContainerBuilder();
        }
    }
}
