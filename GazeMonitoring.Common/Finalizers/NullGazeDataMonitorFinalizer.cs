namespace GazeMonitoring.Common.Finalizers {
    public class NullGazeDataMonitorFinalizer : IGazeDataMonitorFinalizer {
        /// <summary>
        /// Default implementation of finalizing. To override this behavior another implementation can be registered in IoC container
        /// </summary>
        public void FinalizeMonitoring() {
            
        }
    }
}
