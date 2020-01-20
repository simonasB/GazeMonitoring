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

            saccade.Direction = CalculateAngle(deltaX, deltaY);

            var deltaTimeStamp = currentPoint.Timestamp -
                                 previousPoint.Timestamp;

            saccade.Velocity = deltaTimeStamp == 0
                ? saccade.Amplitude
                : saccade.Amplitude / deltaTimeStamp;

            return saccade;
        }

        private static double CalculateAngle(double deltaX, double deltaY)
        {
            // Negate y because the origin is in the top left.
            // Y coordinate is always positive but in the normal origin it is always negative and in 4th quadrant.
            // Might seem weird but Atan2 takes y as the first argument
            var angleInDegrees = RadianToDegree(Math.Atan2(-deltaY, deltaX));

            // Angle in degrees will have value [-180;180]
            // If it's negative, it means that is either in 3rd or 4th quadrant.
            // Need to add 360 to convert to 360 degree range.
            if (angleInDegrees < 0.0)
            {
                angleInDegrees += 360;
            }

            return angleInDegrees;
        }

        private static double RadianToDegree(double angle) {
            return angle * (180.0 / Math.PI);
        }

        private static void Validate(GazePoint previousPoint, GazePoint currentPoint) {
            if (previousPoint == null) {
                throw new ArgumentNullException(nameof(previousPoint));
            }

            if (currentPoint == null) {
                throw new ArgumentNullException(nameof(currentPoint));
            }
        }
    }
}
