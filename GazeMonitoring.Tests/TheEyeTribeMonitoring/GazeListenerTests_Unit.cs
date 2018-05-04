using System;
using NUnit.Framework;
using TheEyeTribeMonitoring;

namespace GazeMonitoring.Tests.TheEyeTribeMonitoring {
    [TestFixture(Category = TestCategory.UNIT)]
    public class GazeListenerTests_Unit {
        [Test]
        public void NullConstructorParameters_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new GazeListener(null));
        }
    }
}
