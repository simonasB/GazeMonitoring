using Autofac;

namespace GazeMonitoring.Data.CustomExample {
    public class ExampleModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<ExampleFileNameFormatter>().As<IFileNameFormatter>();
            builder.RegisterType<ExampleWriter>().As<IExampleWriter>();
            builder.RegisterType<ExampleGazeDataRepository>().As<IGazeDataRepository>();
        }
    }
}
