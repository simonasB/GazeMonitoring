using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using GazeMonitoring.Data;
using GazeMonitoring.Data.Csv;
using GazeMonitoring.Model;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.Csv {
    [TestFixture(Category = TestCategory.INTEGRATION)]
    public class CsvGazeDataRepositoryTests_Integration {
        private readonly Mock<IFileNameFormatter> _mockFileNameFormatter = new Mock<IFileNameFormatter>();
        private const string GazePointsTestCsvFileName = "GzTestFile.csv";
        private const string SaccadesTestCsvFileName = "ScTestFile.csv";
        private readonly string _fullGazePointsCsvFilePath = Path.Combine(Directory.GetCurrentDirectory(), GazePointsTestCsvFileName);
        private readonly string _fullSaccadesCsvFilePath = Path.Combine(Directory.GetCurrentDirectory(), SaccadesTestCsvFileName);

        [OneTimeSetUp]
        public void TestFixtureSetup() {
            _mockFileNameFormatter
                .Setup(o => o.Format(It.Is<FileName>(
                    f => f.DataStream == DataStream.LightlyFilteredGaze.ToString() || f.DataStream == DataStream.UnfilteredGaze.ToString() || f.DataStream == DataStream.SensitiveFixation.ToString() || f.DataStream == DataStream.SlowFixation.ToString())))
                .Returns(GazePointsTestCsvFileName);
            _mockFileNameFormatter.Setup(o => o.Format(It.Is<FileName>(f => f.DataStream == $"{DataStream.SensitiveFixation.ToString()}_Saccades" || f.DataStream == $"{DataStream.SlowFixation.ToString()}_Saccades"))).Returns(SaccadesTestCsvFileName);
        }

        [TearDown]
        public void TestTearDown() {
            if (File.Exists(_fullGazePointsCsvFilePath)) {
                File.Delete(_fullGazePointsCsvFilePath);
            }

            if (File.Exists(_fullSaccadesCsvFilePath)) {
                File.Delete(_fullSaccadesCsvFilePath);
            }
        }

        [Test]
        public void GazePoints_UnfilteredGaze_CorrectlySavedToCsvFile() {
            SaveGazePointsDataToCsvAndAssert(DataStream.UnfilteredGaze);
        }

        [Test]
        public void GazePoints_LightlyFilteredGaze_CorrectlySavedToCsvFile() {
            SaveGazePointsDataToCsvAndAssert(DataStream.LightlyFilteredGaze);
        }

        [Test]
        public void Saccades_GazePoints_SensitiveFixation_CorrectlySavedToCsvFile() {
            SaveGazePointsDataToCsvAndAssert(DataStream.SensitiveFixation);
            SaveSaccadesDataToCsvAndAssert(DataStream.SensitiveFixation);
        }

        [Test]
        public void Saccades_GazePoints_SlowFixation_CorrectlySavedToCsvFile() {
            SaveGazePointsDataToCsvAndAssert(DataStream.SlowFixation);
            SaveSaccadesDataToCsvAndAssert(DataStream.SlowFixation);
        }

        private void SaveGazePointsDataToCsvAndAssert(DataStream dataStream) {
            var subjectInfo = new SubjectInfo {
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

            var csvWritersFactory = new CsvWritersFactory(_mockFileNameFormatter.Object, subjectInfo);
            using (var csvRepository = new CsvGazeDataRepository(csvWritersFactory, dataStream)) {
                foreach (var gazePoint in expectedGazePoints) {
                    csvRepository.SaveGazePoint(gazePoint);
                }
            }

            if (File.Exists(_fullGazePointsCsvFilePath)) {
                using (TextReader reader = File.OpenText(_fullGazePointsCsvFilePath)) {
                    // Skip SubjectInfo header
                    reader.ReadLine();
                    // Skip SubjectInfo data
                    reader.ReadLine();
                    // now initialize the CsvReader
                    var parser = new CsvReader(reader);

                    var actualGazePoints = parser.GetRecords<GazePoint>();

                    Helpers.AssertTwoListsOfGazePoint(expectedGazePoints, actualGazePoints.ToList());
                }

                File.Delete(_fullGazePointsCsvFilePath);
            } else {
                Assert.Fail("Gaze points csv file should have been created.");
            }
        }

        private void SaveSaccadesDataToCsvAndAssert(DataStream dataStream) {
            var subjectInfo = new SubjectInfo {
                Age = 22,
                Details = "Det",
                Name = "Name",
                SessionStartTimestamp = DateTime.UtcNow,
                SessionEndTimeStamp = DateTime.UtcNow,
                SessionId = Guid.NewGuid().ToString()
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

            var csvWritersFactory = new CsvWritersFactory(_mockFileNameFormatter.Object, subjectInfo);
            using (var csvRepository = new CsvGazeDataRepository(csvWritersFactory, dataStream)) {
                foreach (var saccade in expectedSaccades) {
                    csvRepository.SaveSaccade(saccade);
                }
            }

            if (File.Exists(_fullSaccadesCsvFilePath)) {
                using (TextReader reader = File.OpenText(_fullSaccadesCsvFilePath)) {
                    // Skip SubjectInfo header
                    reader.ReadLine();
                    // Skip SubjectInfo data
                    reader.ReadLine();
                    // now initialize the CsvReader
                    var parser = new CsvReader(reader);
                    var actualSaccades = parser.GetRecords<Saccade>();
                    Helpers.AssertTwoListsOfSaccades(expectedSaccades, actualSaccades.ToList());
                }

                File.Delete(_fullSaccadesCsvFilePath);
            } else {
                Assert.Fail("Saccades csv file should have been created.");
            }
        }
    }
}
