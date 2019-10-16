using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.IoC;

namespace MockMonitoring {
    public class MockModule : IoCModule {
        public void Load(IoContainerBuilder builder)
        {
            builder.Register<IGazePointStreamFactory, MockGazePointStreamFactory>();
            builder.Register<IEyeTrackerStatusProvider, MockStatusProvider>();
        }
    }
}
