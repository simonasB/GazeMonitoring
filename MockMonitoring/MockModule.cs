using Autofac;
using GazeMonitoring.Common;
using GazeMonitoring.Common.Streams;

namespace MockMonitoring {
    public class MockModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<MockGazePointStreamFactory>().As<IGazePointStreamFactory>();
        }
    }
}
