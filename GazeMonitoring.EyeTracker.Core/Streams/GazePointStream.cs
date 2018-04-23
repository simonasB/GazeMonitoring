﻿using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.EyeTracker.Core.Streams {
    public abstract class GazePointStream {
        public event EventHandler<GazePointReceivedEventArgs> GazePointReceived;

        protected virtual void OnGazePointReceived(GazePointReceivedEventArgs e) {
            GazePointReceived?.Invoke(this, e);
        }
    }
}