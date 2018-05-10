using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GazeMonitoring.Data;
using GazeMonitoring.Data.Xml;
using GazeMonitoring.Model;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.Xml {
    [TestFixture(Category = TestCategory.INTEGRATION)]
    public class XmlGazeDataRepositoryTests_Integration {
        private readonly Mock<IFileNameFormatter> _mockFileNameFormatter = new Mock<IFileNameFormatter>();
        private const string GazePointsTestXmlFileName = "GzTestFile.xml";
        private const string SaccadesTestXmlFileName = "ScTestFile.xml";
        private readonly string _fullGazePointsXmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), GazePointsTestXmlFileName);
        private readonly string _fullSaccadesXmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), SaccadesTestXmlFileName);

        [OneTimeSetUp]
        public void TestFixtureSetup() {
            _mockFileNameFormatter
                .Setup(o => o.Format(It.Is<FileName>(
                    f => f.DataStream == DataStream.LightlyFilteredGaze.ToString() || f.DataStream == DataStream.UnfilteredGaze.ToString() ||
                         f.DataStream == DataStream.SensitiveFixation.ToString() || f.DataStream == DataStream.SlowFixation.ToString())))
                .Returns(GazePointsTestXmlFileName);
            _mockFileNameFormatter
                .Setup(o => o.Format(It.Is<FileName>(f =>
                    f.DataStream == $"{DataStream.SensitiveFixation.ToString()}_Saccades" || f.DataStream == $"{DataStream.SlowFixation.ToString()}_Saccades")))
                .Returns(SaccadesTestXmlFileName);
        }

        [TearDown]
        public void TestTearDown() {
            if (File.Exists(_fullGazePointsXmlFilePath)) {
                File.Delete(_fullGazePointsXmlFilePath);
            }

            if (File.Exists(_fullSaccadesXmlFilePath)) {
                File.Delete(_fullSaccadesXmlFilePath);
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
            var expectedSubjectInfo = new SubjectInfo {
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

            var xmlWritersFactory = new XmlWritersFactory(_mockFileNameFormatter.Object, expectedSubjectInfo);
            using (var xmlGazeDataRepository = new XmlGazeDataRepository(xmlWritersFactory, dataStream)) {
                foreach (var gazePoint in expectedGazePoints) {
                    xmlGazeDataRepository.SaveGazePoint(gazePoint);
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(GazePointsData));
            if (File.Exists(_fullGazePointsXmlFilePath)) {
                using (TextReader reader = File.OpenText(_fullGazePointsXmlFilePath)) {
                    var gazePointsData = (GazePointsData)serializer.Deserialize(reader);

                    Helpers.AssertTwoListsOfGazePoint(expectedGazePoints, gazePointsData.GazePoints);
                    Helpers.AssertTwoSubjectInfoObjects(expectedSubjectInfo, gazePointsData.SubjectInfo);
                }

                File.Delete(_fullGazePointsXmlFilePath);
            } else {
                Assert.Fail("Gaze points xml file should have been created.");
            }
        }

        private void SaveSaccadesDataToCsvAndAssert(DataStream dataStream) {
            var expectedSubjectInfo = new SubjectInfo {
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

            var csvWritersFactory = new XmlWritersFactory(_mockFileNameFormatter.Object, expectedSubjectInfo);
            using (var csvRepository = new XmlGazeDataRepository(csvWritersFactory, dataStream)) {
                foreach (var saccade in expectedSaccades) {
                    csvRepository.SaveSaccade(saccade);
                }
            }
            XmlSerializer serializer = new XmlSerializer(typeof(SaccadesData));

            if (File.Exists(_fullSaccadesXmlFilePath)) {
                using (TextReader reader = File.OpenText(_fullSaccadesXmlFilePath)) {
                    var gazePointsData = (SaccadesData)serializer.Deserialize(reader);

                    Helpers.AssertTwoListsOfSaccades(expectedSaccades, gazePointsData.Saccades);
                    Helpers.AssertTwoSubjectInfoObjects(expectedSubjectInfo, gazePointsData.SubjectInfo);
                }

                File.Delete(_fullSaccadesXmlFilePath);
            } else {
                Assert.Fail("Saccades xml file should have been created.");
            }
        }
    }
}
