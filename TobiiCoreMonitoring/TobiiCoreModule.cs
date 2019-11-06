using GazeMonitoring.EyeTracker.Core.Calibration;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.IoC;
using Tobii.Interaction;

namespace TobiiCoreMonitoring {
    public class TobiiCoreModule : IoCModule {
        public void Load(IoContainerBuilder builder)
        {
            builder.RegisterSingleton(new Host());
            builder.Register<IGazePointStreamFactory, TobiiCoreGazePointStreamFactory>();
            builder.Register<IEyeTrackerStatusProvider, TobiiStatusProvider>();
            builder.Register<ICalibrationManager, TobiiCalibrationManager>();
        }
    }
}
