using System.Collections.Generic;
using GazeMonitoring.Model;
using NUnit.Framework;

namespace GazeMonitoring.Tests {
    public static class Helpers {
        public static void AssertTwoListsOfGazePoint(List<GazePoint> expectedGazePoints, List<GazePoint> actualGazePoints) {
            Assert.AreEqual(expectedGazePoints.Count, actualGazePoints.Count, "GazePoints count should match.");

            for (int i = 0; i < expectedGazePoints.Count; i++) {
                AssertTwoGazePoints(expectedGazePoints[i], actualGazePoints[i]);
            }
        }

        public static void AssertTwoGazePoints(GazePoint expectedGazePoint, GazePoint actualGazePoint) {
            const double delta = 0.001;
            Assert.AreEqual(expectedGazePoint.X, expectedGazePoint.X, delta, "X values should match");
            Assert.AreEqual(expectedGazePoint.Y, expectedGazePoint.Y, delta, "Y values should match");
            Assert.AreEqual(expectedGazePoint.Timestamp, expectedGazePoint.Timestamp, "Timestamp values should match");
        }

        public static void AssertTwoSaccades(Saccade expectedSaccade, Saccade actualSaccade) {
            const double delta = 0.001;

            Assert.AreEqual(expectedSaccade.Amplitude, actualSaccade.Amplitude, delta, "#1");
            Assert.AreEqual(expectedSaccade.Direction, actualSaccade.Direction, delta, "#2");
            Assert.AreEqual(expectedSaccade.StartTimeStamp, actualSaccade.StartTimeStamp, delta, "#3");
            Assert.AreEqual(expectedSaccade.EndTimeStamp, actualSaccade.EndTimeStamp, "#4");
            Assert.AreEqual(expectedSaccade.Velocity, actualSaccade.Velocity, delta, "#5");
        }

        public static void AssertTwoListsOfSaccades(List<Saccade> expectedSaccades, List<Saccade> actualSaccades) {
            Assert.AreEqual(expectedSaccades.Count, actualSaccades.Count, "GazePoints count should match.");

            for (int i = 0; i < expectedSaccades.Count; i++) {
                AssertTwoSaccades(expectedSaccades[i], actualSaccades[i]);
            }
        }
    }
}
