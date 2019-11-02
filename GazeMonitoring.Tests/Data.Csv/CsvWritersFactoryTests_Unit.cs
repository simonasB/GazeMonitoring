//using System;
//using GazeMonitoring.Data;
//using GazeMonitoring.Data.Csv;
//using GazeMonitoring.Model;
//using Moq;
//using NUnit.Framework;

//namespace GazeMonitoring.Tests.Data.Csv {
//    [TestFixture(Category = TestCategory.UNIT)]
//    public class CsvWritersFactoryTests_Unit {
//        [Test]
//        public void NullConstructorParameters_ThrowsException() {
//            Assert.Throws<ArgumentNullException>(() => new CsvWritersFactory(null, new SubjectInfo()));
//            Assert.Throws<ArgumentNullException>(() => new CsvWritersFactory(null, null));
//            Assert.Throws<ArgumentNullException>(() => new CsvWritersFactory(new Mock<IFileNameFormatter>().Object, null));
//        }
//    }
//}
