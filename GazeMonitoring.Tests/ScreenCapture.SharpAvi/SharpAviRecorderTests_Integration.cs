using System.IO;
using System.Threading;
using GazeMonitoring.Model;
using GazeMonitoring.ScreenCapture;
using GazeMonitoring.ScreenCapture.SharpAvi;
using MockMonitoring;
using NUnit.Framework;

namespace GazeMonitoring.Tests.ScreenCapture.SharpAvi {
    [TestFixture(Category = TestCategory.INTEGRATION)]
    public class SharpAviRecorderTests_Integration {
        private const string TestFileName = "integration_test.avi";
        private static readonly string _testFilePath = Path.Combine(Directory.GetCurrentDirectory(), TestFileName);

        [TearDown]
        public void TestTearDown() {
            if (File.Exists(_testFilePath)) {
                File.Delete(_testFilePath);
            }
        }

        /*[Test]
        public void ValidateAviFileCreated() {
            var sharpAviRecorder = new SharpAviRecorder(new RecorderParams(TestFileName, 10, 50), new AviVideoStreamFactory(),
                new MockGazePointStreamFactory().GetGazePointStream(DataStream.LightlyFilteredGaze));

            sharpAviRecorder.StartRecording();
            Thread.Sleep(1000);
            sharpAviRecorder.StopRecording();

            Assert.True(File.Exists(Path.Combine(Directory.GetCurrentDirectory(), TestFileName)), "Avi file does not exist");
        }*/
    }
}
