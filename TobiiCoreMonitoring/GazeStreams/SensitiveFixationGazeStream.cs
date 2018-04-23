﻿using GazeMonitoring.EyeTracker.Core.Streams;
using GazeMonitoring.Model;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace TobiiCoreMonitoring.GazeStreams {
    public sealed class SensitiveFixationGazeStream : GazePointStream {
        public SensitiveFixationGazeStream(Host host) {
            host.Streams.CreateFixationDataStream(FixationDataMode.Sensitive).Next += (sender, data) => {
                OnGazePointReceived(new GazePointReceivedEventArgs {
                    GazePoint = new GazePoint {
                        X = data.Data.X,
                        Y = data.Data.Y,
                        Timestamp = (long)(data.Data.Timestamp * 1000)
                    }
                });
            };
        }
    }
}
