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
    }
}
