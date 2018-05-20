using Autofac;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.EyeTracker.Core.Streams;

namespace MockMonitoring {
    public class MockModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<MockGazePointStreamFactory>().As<IGazePointStreamFactory>();
            builder.RegisterType<MockStatusProvider>().As<IEyeTrackerStatusProvider>();
        }
    }
}
