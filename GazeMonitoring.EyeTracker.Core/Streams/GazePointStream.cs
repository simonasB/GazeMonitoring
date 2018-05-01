using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.EyeTracker.Core.Streams {
    public abstract class GazePointStream {
        public event EventHandler<GazePointReceivedEventArgs> GazePointReceived;

        protected virtual void OnGazePointReceived(GazePointReceivedEventArgs e) {
            if (!double.IsNaN(e.GazePoint.X) && !double.IsNaN(e.GazePoint.Y)) {
                GazePointReceived?.Invoke(this, e);
            }
        }
    }
}
