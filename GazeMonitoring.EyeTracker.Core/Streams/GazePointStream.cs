using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.EyeTracker.Core.Streams {
    /// <summary>
    /// Provides abstraction to subscribe to gaze points stream from eyetracker
    /// </summary>
    public abstract class GazePointStream {
        /// <summary>
        /// This event raised by eyetracker specific implementation by passing gaze points in realtime
        /// </summary>
        public event EventHandler<GazePointReceivedEventArgs> GazePointReceived;

        /// <summary>
        /// Invoked when event about gaze point is received, filters not valid gaze points.
        /// </summary>
        /// <param name="e">Gazepoint data</param>
        protected virtual void OnGazePointReceived(GazePointReceivedEventArgs e) {
            if (!double.IsNaN(e.GazePoint.X) && !double.IsNaN(e.GazePoint.Y)) {
                GazePointReceived?.Invoke(this, e);
            }
        }
    }
}
