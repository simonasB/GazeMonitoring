using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GazeMonitoring.Data.Csv;
using GazeMonitoring.Data.PostgreSQL;
using GazeMonitoring.Logging;
using GazeMonitoring.Model;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.PostgreSQL {
    [TestFixture(Category = TestCategory.INTEGRATION)]
    public class PostgreSQLGazeMonitorFinalizerTests_Integration {
        private DatabaseRepository _databaseRepository;
        private readonly Mock<ILoggerFactory> _mockLoggerFactory = new Mock<ILoggerFactory>();
        [OneTimeSetUp]
        public void TestFixtureSetup() {
            var connectionString = ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString;
            _mockLoggerFactory.Setup(o => o.GetLogger(It.IsAny<Type>())).Returns(new Mock<ILogger>().Object);
            _databaseRepository = new DatabaseRepository(connectionString, _mockLoggerFactory.Object);
        }

        [Test]
        public void FinalizeMonitoring_WorksAsExpected() {
            var subjectInfo = new SubjectInfo
            {
                Age = 22,
                Details = "Det",
                Name = "Name",
                SessionStartTimestamp = DateTime.UtcNow,
                SessionEndTimeStamp = DateTime.UtcNow,
                SessionId = Guid.NewGuid().ToString()
            };

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

            var csvWritersFactory = new CsvWritersFactory(new PostgreSQLFileNameFormatter(), subjectInfo);
            using (var csvRepository = new CsvGazeDataRepository(csvWritersFactory, DataStream.SlowFixation)) {
                foreach (var gazePoint in expectedGazePoints) {
                    csvRepository.SaveOne(gazePoint);
                }

                expectedSaccades.ForEach(saccade => {
                    csvRepository.SaveOne(saccade);
                });
            }

            var finalizer = new PostgreSQLGazeDataMonitorFinalizer(_databaseRepository, subjectInfo, _mockLoggerFactory.Object);
            finalizer.FinalizeMonitoring();

            var savedSubjectInfo = _databaseRepository.RetrieveSubjectInfo(subjectInfo.SessionId);
            Helpers.AssertTwoSubjectInfoObjects(subjectInfo, savedSubjectInfo);

            var actualDbGazePoints = _databaseRepository.SelectGazePoints(savedSubjectInfo.Id.Value);
            var expectedDbGazePoints = expectedGazePoints.Select(o => new DbGazePoint
            {
                X = o.X,
                Y = o.Y,
                Timestamp = o.Timestamp,
                SessionId = subjectInfo.SessionId,
                SubjectInfoId = savedSubjectInfo.Id.Value
            }).ToList();
            Helpers.AssertTwoListsOfDbGazePoints(actualDbGazePoints, expectedDbGazePoints);

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

            _databaseRepository.DeleteGazePoints(savedSubjectInfo.Id.Value);
            _databaseRepository.DeleteSaccades(savedSubjectInfo.Id.Value);
            _databaseRepository.DeleteSubjectInfo(subjectInfo.SessionId);
        }
    }
}
