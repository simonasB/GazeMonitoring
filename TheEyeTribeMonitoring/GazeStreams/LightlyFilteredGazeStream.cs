﻿using EyeTribe.ClientSdk.Data;
using GazeMonitoring.Common.Streams;
using GazeMonitoring.Model;

namespace TheEyeTribeMonitoring.GazeStreams {
    public class LightlyFilteredGazeStream : GazePointStream, IFilteredGazeDataPublisher {
        public void PublishFilteredData(GazeData gazeData) {
            OnGazePointReceived(new GazePointReceivedEventArgs {
                GazePoint = new GazePoint {
                    Timestamp = gazeData.TimeStamp,
                    X = gazeData.RawCoordinates.X,
                    Y = gazeData.RawCoordinates.Y
                }
            });
        }
    }
}
