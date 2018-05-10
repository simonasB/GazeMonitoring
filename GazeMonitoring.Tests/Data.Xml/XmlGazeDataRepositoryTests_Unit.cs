using System;
using GazeMonitoring.Data.Xml;
using GazeMonitoring.Model;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.Xml {
    [TestFixture(Category = TestCategory.UNIT)]
    public class XmlGazeDataRepositoryTests_Unit {
        [Test]
        public void NullConstructorParameters_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new XmlGazeDataRepository(null, DataStream.UnfilteredGaze));
        }

        [Test]
        public void SaveGazePoint_Null_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new XmlGazeDataRepository(new Mock<IXmlWritersFactory>().Object, DataStream.UnfilteredGaze).SaveGazePoint(null));
        }

        [Test]
        public void SaveSaccade_Null_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new XmlGazeDataRepository(new Mock<IXmlWritersFactory>().Object, DataStream.UnfilteredGaze).SaveSaccade(null));
        }
    }
}
