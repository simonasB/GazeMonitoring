using System;
using GazeMonitoring.Data.Writers;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Monitor;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.GazeMonitoring.Wpf {
    [TestFixture(Category = TestCategory.UNIT)]
    public class GazeDataMonitorTests_Unit {
        [Test]
        public void NullConstructorParameters_ThrowsException() {
            //Assert.Throws<ArgumentNullException>(() => new GazeDataMonitor(null, null));
            //Assert.Throws<ArgumentNullException>(() => new GazeDataMonitor(new Mock<GazePointStream>().Object, null));
            //Assert.Throws<ArgumentNullException>(() => new GazeDataMonitor(null, new Mock<IGazeDataWriter>().Object));
        }
    }
}
