using System;
using NUnit.Framework;
using TobiiCoreMonitoring;
using GazeMonitoring.Model;

namespace GazeMonitoring.Tests.TobiiCoreMonitoring {
    [TestFixture(Category = TestCategory.UNIT)]
    public class TobiiCoreGazePointStreamFactoryTest_Unit {
        [Test]
        public void NullConstructorParameters_ThrowsException() {
           // Assert.Throws<ArgumentNullException>(() => new TobiiCoreGazePointStreamFactory(null, (IScreenParameters)null));
        }
    }
}