using System;

namespace GazeMonitoring.Common.Entities {
    public class GazePointReceivedEventArgs : EventArgs {
        public GazePoint GazePoint { get; set; }
    }
}
