using System;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Common {
    public abstract class GazePointStream {
        public event EventHandler<GazePointReceivedEventArgs> GazePointReceived;

        protected virtual void OnGazePointReceived(GazePointReceivedEventArgs e) {
            GazePointReceived?.Invoke(this, e);
        }
    }
}
