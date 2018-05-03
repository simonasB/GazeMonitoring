using System;
using GazeMonitoring.Data.Writers;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.Writers {
    [TestFixture(Category = TestCategory.UNIT)]
    public class MultipleSourcesGazeDataWriterTests_Unit {
        [Test]
        public void NullConstructorParameters_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new MultipleSourcesGazeDataWriter(null));
        }
    }
}
