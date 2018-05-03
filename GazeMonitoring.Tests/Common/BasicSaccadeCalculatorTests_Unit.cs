using System;
using GazeMonitoring.Common.Calculations;
using GazeMonitoring.Model;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Common {
    [TestFixture(Category = TestCategory.UNIT)]
    public class BasicSaccadeCalculatorTests_Unit {
        private readonly BasicSaccadeCalculator _saccadeCalculator = new BasicSaccadeCalculator();

        [Test]
        public void Calculate_ThrowsArgumentNullException_WhenGazePointNull() {
            Assert.Throws<ArgumentNullException>(() => _saccadeCalculator.Calculate(null, new GazePoint()));
            Assert.Throws<ArgumentNullException>(() => _saccadeCalculator.Calculate(new GazePoint(), null));
        }

        [Test]
        public void Calculate_CorrectSaccade_TimestampDiffersBetweenPoints() {
            var previousTimestamp = 100;

            var previousGazePoint = new GazePoint {
                X = 1000,
                Y = 1200,
                Timestamp = previousTimestamp
            };

            var currentTimeStamp = previousTimestamp + 10;

            var currentPoint = new GazePoint {
                X = 990,
                Y = 1190,
                Timestamp = currentTimeStamp
            };

            var expectedSaccade = new Saccade {
                Amplitude = 14.1421,
                Direction = 45.0000,
                StartTimeStamp = previousTimestamp,
                EndTimeStamp = currentTimeStamp,
                Velocity = 1.41421
            };

            var calculatedSaccade = _saccadeCalculator.Calculate(previousGazePoint, currentPoint);

            Helpers.AssertTwoSaccades(expectedSaccade, calculatedSaccade);
        }

        [Test]
        public void Calculate_CorrectSaccade_SameTimestampBetweenPoints() {
            var previousTimestamp = 100;

            var previousGazePoint = new GazePoint {
                X = 1000,
                Y = 1200,
                Timestamp = previousTimestamp
            };

            var currentPoint = new GazePoint {
                X = 990,
                Y = 1190,
                Timestamp = previousTimestamp
            };

            var expectedSaccade = new Saccade {
                Amplitude = 14.1421,
                Direction = 45.0000,
                StartTimeStamp = previousTimestamp,
                EndTimeStamp = previousTimestamp,
                Velocity = 14.1421
            };

            var calculatedSaccade = _saccadeCalculator.Calculate(previousGazePoint, currentPoint);      

            Helpers.AssertTwoSaccades(expectedSaccade, calculatedSaccade);
        }
    }
}
