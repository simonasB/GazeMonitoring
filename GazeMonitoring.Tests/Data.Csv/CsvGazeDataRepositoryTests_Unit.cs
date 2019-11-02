//using System;
//using GazeMonitoring.Data.Csv;
//using GazeMonitoring.Model;
//using Moq;
//using NUnit.Framework;

//namespace GazeMonitoring.Tests.Data.Csv {
//    [TestFixture(Category = TestCategory.UNIT)]
//    public class CsvGazeDataRepositoryTests_Unit {
//        [Test]
//        public void NullConstructorParameters_ThrowsException() {
//            Assert.Throws<ArgumentNullException>(() => new CsvGazeDataRepository(null, DataStream.UnfilteredGaze));
//        }

//        [Test]
//        public void SaveGazePoint_Null_ThrowsException() {
//            Assert.Throws<ArgumentNullException>(() => new CsvGazeDataRepository(new Mock<ICsvWritersFactory>().Object, DataStream.UnfilteredGaze).SaveGazePoint(null));
//        }

//        [Test]
//        public void SaveSaccade_Null_ThrowsException() {
//            Assert.Throws<ArgumentNullException>(() => new CsvGazeDataRepository(new Mock<ICsvWritersFactory>().Object, DataStream.UnfilteredGaze).SaveSaccade(null));
//        }
//    }
//}
