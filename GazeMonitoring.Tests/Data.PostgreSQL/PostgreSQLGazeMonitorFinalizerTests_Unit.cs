//using System;
//using GazeMonitoring.Data.PostgreSQL;
//using GazeMonitoring.Logging;
//using GazeMonitoring.Model;
//using Moq;
//using NUnit.Framework;

//namespace GazeMonitoring.Tests.Data.PostgreSQL {
//    [TestFixture(Category = TestCategory.UNIT)]
//    public class PostgreSQLGazeMonitorFinalizerTests_Unit {
//        [Test]
//        public void NullConstructorParameters_ThrowsException() {
//            Assert.Throws<ArgumentNullException>(() => new PostgreSQLGazeDataMonitorFinalizer(null, null, new Mock<ILoggerFactory>().Object));
//            Assert.Throws<ArgumentNullException>(() => new PostgreSQLGazeDataMonitorFinalizer(null, new SubjectInfo(), null));
//            Assert.Throws<ArgumentNullException>(() => new PostgreSQLGazeDataMonitorFinalizer(new Mock<IDatabaseRepository>().Object, null, null));
//            Assert.Throws<ArgumentNullException>(() => new PostgreSQLGazeDataMonitorFinalizer(new Mock<IDatabaseRepository>().Object, new SubjectInfo(), null));
//            Assert.Throws<ArgumentNullException>(() => new PostgreSQLGazeDataMonitorFinalizer(null, new SubjectInfo(), new Mock<ILoggerFactory>().Object));
//            Assert.Throws<ArgumentNullException>(() => new PostgreSQLGazeDataMonitorFinalizer(null, null, null));
//        }


//    }
//}
