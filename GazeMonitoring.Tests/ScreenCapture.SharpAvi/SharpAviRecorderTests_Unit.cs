using System;
using GazeMonitoring.Data.Xml;
using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;
using GazeMonitoring.ScreenCapture;
using GazeMonitoring.ScreenCapture.SharpAvi;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.ScreenCapture.SharpAvi {
    [TestFixture(Category = TestCategory.UNIT)]
    public class SharpAviRecorderTests_Unit {
        [Test]
        public void NullConstructorParameters_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new SharpAviRecorder(null, null, null));
            Assert.Throws<ArgumentNullException>(() => new SharpAviRecorder(new RecorderParams("a", 10, 50), null, null));
            Assert.Throws<ArgumentNullException>(() => new SharpAviRecorder(new RecorderParams("a", 10, 50), new AviVideoStreamFactory(), null));
            Assert.Throws<ArgumentNullException>(() => new SharpAviRecorder(null, new AviVideoStreamFactory(), new Mock<GazePointStream>().Object));
        }
    }
}
