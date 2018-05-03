using System;
using GazeMonitoring.Data;
using GazeMonitoring.Data.Writers;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.Writers {
    [TestFixture(Category = TestCategory.UNIT)]
    public class GazePointWriterTests_Unit {
        private readonly Mock<IGazeDataRepository> _mockGazeDataRepository = new Mock<IGazeDataRepository>();
        private GazePointWriter _gazePointWriter;

        [OneTimeSetUp]
        public void SetUpFixture() {
            _gazePointWriter = new GazePointWriter(_mockGazeDataRepository.Object);
        }

        [Test]
        public void NullGazeDataRepository_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new GazePointWriter(null));
        }

        [Test]
        public void Write_NullGazePoint_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new GazePointWriter(null).Write(null));
        }
    }
}
