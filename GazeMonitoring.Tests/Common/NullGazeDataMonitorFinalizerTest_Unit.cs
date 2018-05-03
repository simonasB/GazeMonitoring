using GazeMonitoring.Common.Finalizers;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Common {
    [TestFixture(Category = TestCategory.UNIT)]
    public class NullGazeDataMonitorFinalizerTest_Unit {
        private readonly NullGazeDataMonitorFinalizer _finalizer = new NullGazeDataMonitorFinalizer();

        [Test(Description = "Ensure that null finalizer is just a null implementation that does not break application.")]
        public void Finalize_DoesNotThrowAnyException() {
            Assert.DoesNotThrow(() => _finalizer.FinalizeMonitoring());
        }
    }
}
