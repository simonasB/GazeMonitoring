using System;
using GazeMonitoring.Data.PostgreSQL;
using GazeMonitoring.Logging;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.PostgreSQL {
    [TestFixture(Category = TestCategory.UNIT)]
    public class DatabaseRepositoryTests_Unit {
        [Test]
        public void NullConstructorParameters_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new DatabaseRepository(null, null));
            Assert.Throws<ArgumentNullException>(() => new DatabaseRepository(null, new Mock<ILoggerFactory>().Object));
            Assert.Throws<ArgumentNullException>(() => new DatabaseRepository("cs", null));
        }

        [Test]
        public void RetrieveSubjectInfo_NullSessionId_ThrowsException() {
            var databaseRepository = new DatabaseRepository("cs", new Mock<ILoggerFactory>().Object);

            Assert.Throws<ArgumentNullException>(() => databaseRepository.RetrieveSubjectInfo(null));
        }

        [Test]
        public void SaveSubjectInfo_NullSubjectInfo_ThrowsException() {
            var databaseRepository = new DatabaseRepository("cs", new Mock<ILoggerFactory>().Object);

            Assert.Throws<ArgumentNullException>(() => databaseRepository.SaveSubjectInfo(null));
        }

        [Test]
        public void BinaryInsertGazePoints_NullGazePoints_ThrowsException() {
            var databaseRepository = new DatabaseRepository("cs", new Mock<ILoggerFactory>().Object);

            Assert.Throws<ArgumentNullException>(() => databaseRepository.BinaryInsertGazePoints(null, "", 0, DateTime.MinValue));
        }

        [Test]
        public void BinaryInsertSaccades_NullSaccades_ThrowsException() {
            var databaseRepository = new DatabaseRepository("cs", new Mock<ILoggerFactory>().Object);

            Assert.Throws<ArgumentNullException>(() => databaseRepository.BinaryInsertSaccades(null, "", 0, DateTime.MinValue));
        }
    }
}
