using Autofac;
using GazeMonitoring.Common;

namespace MockMonitoring {
    public class MockModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<MockGazePointStreamFactory>().As<IGazePointStreamFactory>();
        }
    }
}
