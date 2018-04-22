using System;

namespace GazeMonitoring.Model {
    public class GazePointReceivedEventArgs : EventArgs {
        public GazePoint GazePoint { get; set; }
    }
}
