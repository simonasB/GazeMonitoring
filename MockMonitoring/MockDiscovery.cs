using System;
using GazeMonitoring.EyeTracker.Core.Calibration;
using GazeMonitoring.EyeTracker.Core.Discovery;
using GazeMonitoring.EyeTracker.Core.Status;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.IoC;

namespace MockMonitoring {
    public class MockDiscovery : IDiscoverable {
        public DiscoveryResult Discover(IoContainerBuilder container) {
            if (container == null) {
                throw new ArgumentNullException(nameof(container));
            }

            container.Register<IGazePointStreamFactory, MockGazePointStreamFactory>();
            container.Register<IEyeTrackerStatusProvider, MockStatusProvider>();
            container.Register<ICalibrationManager, MockCalibrationManager>();

            return new DiscoveryResult {
                IsActive = true
            };
        }
    }
}
