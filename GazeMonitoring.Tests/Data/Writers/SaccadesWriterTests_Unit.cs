using System;
using GazeMonitoring.Common.Calculations;
using GazeMonitoring.Data;
using GazeMonitoring.Data.Writers;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.Writers {
    [TestFixture(Category = TestCategory.UNIT)]
    public class SaccadesWriterTests_Unit {
        private readonly Mock<IGazeDataRepository> _mockGazeDataRepository = new Mock<IGazeDataRepository>();
        private readonly Mock<ISaccadeCalculator> _mockSaccadeCalculator = new Mock<ISaccadeCalculator>();

        [Test]
        public void NullConstructorParameters_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new SaccadesWriter(null, null));
            Assert.Throws<ArgumentNullException>(() => new SaccadesWriter(_mockGazeDataRepository.Object, null));
            Assert.Throws<ArgumentNullException>(() => new SaccadesWriter(null, _mockSaccadeCalculator.Object));
        }

        [Test]
        public void Write_NullGazePoint_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new SaccadesWriter(_mockGazeDataRepository.Object, _mockSaccadeCalculator.Object).Write(null));
        }
    }
}
