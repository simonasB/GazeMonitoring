using System;
using GazeMonitoring.ScreenCapture;
using NUnit.Framework;

namespace GazeMonitoring.Tests.ScreenCapture {
    [TestFixture(Category = TestCategory.UNIT)]
    public class RecorderParamsTests_Unit {
        [Test]
        public void RecordersParams_FileNameNull_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new RecorderParams(null, 10, 10));
        }
    }
}
