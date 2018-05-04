using System;
using System.Collections.Generic;
using GazeMonitoring.Data.PostgreSQL;
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

        public static void AssertTwoSubjectInfoObjects(SubjectInfo expectedSubjectInfo, SubjectInfo actualSubjectInfo) {
            Assert.AreEqual(expectedSubjectInfo.Age, actualSubjectInfo.Age, "Age should match");
            Assert.AreEqual(expectedSubjectInfo.Details, actualSubjectInfo.Details, "Details should match");
            Assert.AreEqual(expectedSubjectInfo.Name, actualSubjectInfo.Name, "Name should match");
            Assert.AreEqual(GetUnixTimeSpan(expectedSubjectInfo.SessionEndTimeStamp).TotalSeconds, GetUnixTimeSpan(actualSubjectInfo.SessionEndTimeStamp).TotalSeconds, 0.1,
                "SessionEndTimeStamp should match");
            Assert.AreEqual(expectedSubjectInfo.SessionId, actualSubjectInfo.SessionId, "SessionId should match");
            Assert.AreEqual(GetUnixTimeSpan(expectedSubjectInfo.SessionStartTimestamp).TotalSeconds, GetUnixTimeSpan(actualSubjectInfo.SessionStartTimestamp).TotalSeconds, 0.1,
                "SessionStartTimestamp should match");
        }

        private static TimeSpan GetUnixTimeSpan(DateTime dateTime) {
            return dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }
        public static void AssertTwoListsOfDbGazePoints(List<DbGazePoint> expectedList, List<DbGazePoint> actualList) {
            Assert.AreEqual(expectedList.Count, actualList.Count, "Count should match");

            void AssertTwoDbGazePoints(DbGazePoint expectedDbGazePoint, DbGazePoint actualDbGazePoint) {
                const double delta = 0.001;
                Assert.AreEqual(expectedDbGazePoint.X, actualDbGazePoint.X, delta, "X should match");
                Assert.AreEqual(expectedDbGazePoint.Y, actualDbGazePoint.Y, delta, "Y should match");
                Assert.AreEqual(expectedDbGazePoint.SessionId, actualDbGazePoint.SessionId, "SessionId should match");
                Assert.AreEqual(expectedDbGazePoint.SubjectInfoId, actualDbGazePoint.SubjectInfoId, "SubjectInfoId should match");
                Assert.AreEqual(expectedDbGazePoint.Timestamp, actualDbGazePoint.Timestamp, "Timestamp should match");
            }

            for (int i = 0; i < expectedList.Count; i++) {
                AssertTwoDbGazePoints(expectedList[i], actualList[i]);
            }
        }

        public static void AssertTwoListsOfDbSaccades(List<DbSaccade> expectedList, List<DbSaccade> actualList) {
            Assert.AreEqual(expectedList.Count, actualList.Count, "Count should match");

            void AssertTwoDbSaccades(DbSaccade expectedDbGazePoint, DbSaccade actualDbGazePoint) {
                const double delta = 0.001;
                Assert.AreEqual(expectedDbGazePoint.Direction, actualDbGazePoint.Direction, delta, "Direction should match");
                Assert.AreEqual(expectedDbGazePoint.Velocity, actualDbGazePoint.Velocity, delta, "Velocity should match");
                Assert.AreEqual(expectedDbGazePoint.Amplitude, actualDbGazePoint.Amplitude, delta, "Amplitude should match");
                Assert.AreEqual(expectedDbGazePoint.SessionId, actualDbGazePoint.SessionId, "SessionId should match");
                Assert.AreEqual(expectedDbGazePoint.SubjectInfoId, actualDbGazePoint.SubjectInfoId, "SubjectInfoId should match");
                Assert.AreEqual(expectedDbGazePoint.StartTimeStamp, actualDbGazePoint.StartTimeStamp, "StartTimeStamp should match");
                Assert.AreEqual(expectedDbGazePoint.EndTimeStamp, actualDbGazePoint.EndTimeStamp, "EndTimeStamp should match");
            }

            for (int i = 0; i < expectedList.Count; i++) {
                AssertTwoDbSaccades(expectedList[i], actualList[i]);
            }
        }
    }
}
