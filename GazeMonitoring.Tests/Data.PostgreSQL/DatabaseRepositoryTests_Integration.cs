using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GazeMonitoring.Data.PostgreSQL;
using GazeMonitoring.Logging;
using GazeMonitoring.Model;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.PostgreSQL {
    [TestFixture(Category = TestCategory.INTEGRATION)] 
    public class DatabaseRepositoryTests_Integration {
        private DatabaseRepository _databaseRepository;

        [OneTimeSetUp]
        public void TestFixtureSetup() {
            var connectionString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
            _databaseRepository = new DatabaseRepository(connectionString, new Mock<ILoggerFactory>().Object);
        }

        [Test]
        public void SubjectInfo_Save_Retrieve_Delete_WorksCorrectly() {
            var expectedSubjectInfo = new SubjectInfo {
                Age = 22,
                Details = "integration_test",
                Name = "Simonas",
                SessionStartTimestamp = DateTime.UtcNow,
                SessionEndTimeStamp = DateTime.UtcNow,
                SessionId = Guid.NewGuid().ToString()
            };

            _databaseRepository.SaveSubjectInfo(expectedSubjectInfo);

            var actualSubjectInfo = _databaseRepository.RetrieveSubjectInfo(expectedSubjectInfo.SessionId);

            _databaseRepository.DeleteSubjectInfo(actualSubjectInfo.SessionId);

            Helpers.AssertTwoSubjectInfoObjects(expectedSubjectInfo, actualSubjectInfo);
        }

        [Test]
        public void BinaryInsertGazePoints_WorksCorrectly() {
            var subjectInfo = new SubjectInfo {
                Age = 22,
                Details = "integration_test",
                Name = "Simonas",
                SessionStartTimestamp = DateTime.UtcNow,
                SessionEndTimeStamp = DateTime.UtcNow,
                SessionId = Guid.NewGuid().ToString()
            };

            _databaseRepository.SaveSubjectInfo(subjectInfo);

            var savedSubjectInfo = _databaseRepository.RetrieveSubjectInfo(subjectInfo.SessionId);

            var expectedGazePoints = new List<GazePoint>() {
                new GazePoint {
                    X = 1.1,
                    Y = 1.1,
                    Timestamp = 1
                },
                new GazePoint {
                    X = 2.1,
                    Y = 2.1,
                    Timestamp = 2
                },
                new GazePoint {
                    X = 3.2,
                    Y = 3.2,
                    Timestamp = 3
                },
                new GazePoint {
                    X = 4.5,
                    Y = 4.5,
                    Timestamp = 4
                }
            };

            _databaseRepository.BinaryInsertGazePoints(expectedGazePoints, savedSubjectInfo.SessionId, savedSubjectInfo.Id, DateTime.UtcNow);

            var actualGazePoints = _databaseRepository.SelectGazePoints(savedSubjectInfo.Id.Value);

            var expectedDbGazePoints = expectedGazePoints.Select(o => new DbGazePoint {
                X = o.X,
                Y = o.Y,
                Timestamp = o.Timestamp,
                SessionId = subjectInfo.SessionId,
                SubjectInfoId = savedSubjectInfo.Id.Value
            }).ToList();

            Helpers.AssertTwoListsOfDbGazePoints(expectedDbGazePoints, actualGazePoints);

            _databaseRepository.DeleteGazePoints(savedSubjectInfo.Id.Value);
        }

        [Test]
        public void BinaryInsertSaccades_WorksCorrectly() {
            var subjectInfo = new SubjectInfo {
                Age = 22,
                Details = "integration_test",
                Name = "Simonas",
                SessionStartTimestamp = DateTime.UtcNow,
                SessionEndTimeStamp = DateTime.UtcNow,
                SessionId = Guid.NewGuid().ToString()
            };

            _databaseRepository.SaveSubjectInfo(subjectInfo);

            var savedSubjectInfo = _databaseRepository.RetrieveSubjectInfo(subjectInfo.SessionId);

            var expectedSaccades = new List<Saccade>() {
                new Saccade {
                    Amplitude = 1.2,
                    Direction = 1.3,
                    Velocity = 1.4,
                    StartTimeStamp = 10,
                    EndTimeStamp = 11
                },
                new Saccade {
                    Amplitude = 2.2,
                    Direction = 2.3,
                    Velocity = 2.4,
                    StartTimeStamp = 11,
                    EndTimeStamp = 12
                },
                new Saccade {
                    Amplitude = 3.2,
                    Direction = 3.3,
                    Velocity = 4.4,
                    StartTimeStamp = 12,
                    EndTimeStamp = 13
                }
            };

            _databaseRepository.BinaryInsertSaccades(expectedSaccades, savedSubjectInfo.SessionId, savedSubjectInfo.Id, DateTime.UtcNow);

            var actualDbSaccades = _databaseRepository.SelectSaccades(savedSubjectInfo.Id.Value);

            var expectedDbSaccades = expectedSaccades.Select(o => new DbSaccade
            {
                Amplitude = o.Amplitude,
                Velocity = o.Velocity,
                Direction = o.Direction,
                StartTimeStamp = o.StartTimeStamp,
                EndTimeStamp = o.EndTimeStamp,
                SessionId = subjectInfo.SessionId,
                SubjectInfoId = savedSubjectInfo.Id.Value
            }).ToList();

            Helpers.AssertTwoListsOfDbSaccades(expectedDbSaccades, actualDbSaccades);

            _databaseRepository.DeleteSaccades(savedSubjectInfo.Id.Value);
        }
    }
}
