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
        private const string TestCsvFileName = "TestFile.csv";
        private readonly string _fullCsvFilePath = Path.Combine(Directory.GetCurrentDirectory(), TestCsvFileName);

        [OneTimeSetUp]
        public void TestFixtureSetup() {
            _mockFileNameFormatter.Setup(o => o.Format(It.IsAny<FileName>())).Returns(TestCsvFileName);
        }

        [TearDown]
        public void TestTearDown() {
            if (File.Exists(_fullCsvFilePath)) {
                File.Delete(_fullCsvFilePath);
            }
        }

        [Test]
        public void Test() {
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
                    Timestamp = 1
                },
                new GazePoint {
                    X = 3.2,
                    Y = 3.2,
                    Timestamp = 1
                },
                new GazePoint {
                    X = 4.5,
                    Y = 4.5,
                    Timestamp = 1
                }
            };

            var csvWritersFactory = new CsvWritersFactory(_mockFileNameFormatter.Object, subjectInfo);
            using (var csvRepository = new CsvGazeDataRepository(csvWritersFactory, DataStream.LightlyFilteredGaze)) {
                foreach (var gazePoint in expectedGazePoints) {
                    csvRepository.SaveOne(gazePoint);
                }
            }

            if (File.Exists(_fullCsvFilePath)) {
                using (TextReader reader = File.OpenText(_fullCsvFilePath)) {
                    // Skip SubjectInfo header
                    reader.ReadLine();
                    // Skip SubjectInfo data
                    reader.ReadLine();
                    // now initialize the CsvReader
                    var parser = new CsvReader(reader);

                    var actualGazePoints = parser.GetRecords<GazePoint>();

                    Helpers.AssertTwoListsOfGazePoint(expectedGazePoints, actualGazePoints.ToList());
                }

                File.Delete(_fullCsvFilePath);
            }
        }
    }
}
