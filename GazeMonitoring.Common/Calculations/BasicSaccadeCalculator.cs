using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.Common.Calculations {
    public class BasicSaccadeCalculator : ISaccadeCalculator {
        public Saccade Calculate(GazePoint previousPoint, GazePoint currentPoint) {
            Validate(previousPoint, currentPoint);

            var deltaX = currentPoint.X - previousPoint.X;
            var deltaY = currentPoint.Y - previousPoint.Y;

            var saccade = new Saccade {
                Amplitude = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2)),
                StartTimeStamp = previousPoint.Timestamp,
                EndTimeStamp = currentPoint.Timestamp
            };

            if (Math.Abs(deltaX) > 0.0001) {
                saccade.Direction = RadianToDegree(Math.Atan(Math.Abs(deltaY) /
                                                             Math.Abs(deltaX)));
            } else {
                saccade.Direction = 0;
            }

            var deltaTimeStamp = currentPoint.Timestamp -
                                 previousPoint.Timestamp;

            saccade.Velocity = deltaTimeStamp == 0
                ? saccade.Amplitude
                : saccade.Amplitude / deltaTimeStamp;

            return saccade;
        }

        private double RadianToDegree(double angle) {
            return angle * (180.0 / Math.PI);
        }

        private void Validate(GazePoint previousPoint, GazePoint currentPoint) {
            if (previousPoint == null) {
                throw new ArgumentNullException(nameof(previousPoint));
            }

            if (currentPoint == null) {
                throw new ArgumentNullException(nameof(currentPoint));
            }
        }
    }
}
