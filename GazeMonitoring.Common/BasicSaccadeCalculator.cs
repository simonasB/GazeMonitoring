using System;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Common {
    public class BasicSaccadeCalculator : ISaccadeCalculator {
        public Saccade Calculate(GazePoint previousPoint, GazePoint currentPoint) {
            var deltaX = currentPoint.X - previousPoint.X;
            var deltaY = currentPoint.Y - previousPoint.Y;

            var saccade = new Saccade
            {
                Direction = Math.Atan(Math.Abs(deltaY) /
                                      Math.Abs(deltaX)),
                Amplitude = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2)),
                StartTimeStamp = previousPoint.Timestamp,
                EndTimeStamp = currentPoint.Timestamp
            };

            var deltaTimeStamp = currentPoint.Timestamp -
                                 previousPoint.Timestamp;

            saccade.Velocity = Math.Abs(deltaTimeStamp) < 0.0001
                ? saccade.Amplitude
                : saccade.Amplitude / (deltaTimeStamp);

            return saccade;
        }
    }
}
