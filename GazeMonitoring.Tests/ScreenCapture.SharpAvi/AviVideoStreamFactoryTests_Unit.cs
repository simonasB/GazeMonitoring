using System;
using GazeMonitoring.ScreenCapture;
using GazeMonitoring.ScreenCapture.SharpAvi;
using NUnit.Framework;
using SharpAvi.Output;

namespace GazeMonitoring.Tests.ScreenCapture.SharpAvi {
    [TestFixture(Category = TestCategory.UNIT)]
    public class AviVideoStreamFactoryTests_Unit {
        [Test]
        public void CreateVideoStream_NullArguments_ThrowsException() {
            var factory = new AviVideoStreamFactory();

            Assert.Throws<ArgumentNullException>(() => factory.CreateVideoStream(null, new RecorderParams("file.avi", 10, 50), global::SharpAvi.KnownFourCCs.Codecs.MotionJpeg));
            Assert.Throws<ArgumentNullException>(() => factory.CreateVideoStream(null, null, global::SharpAvi.KnownFourCCs.Codecs.MotionJpeg));
            Assert.Throws<ArgumentNullException>(() => factory.CreateVideoStream(new AviWriter("path"), null, global::SharpAvi.KnownFourCCs.Codecs.MotionJpeg));
        }
    }
}
