using System;
using GazeMonitoring.Data;
using GazeMonitoring.Model;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data {
    [TestFixture(Category = TestCategory.UNIT)]
    public class StandardFileNameFormatterTests_Unit {
        private readonly StandardFileNameFormatter _formatter = new StandardFileNameFormatter();
        [Test]
        public void Format_FileNameArgumentNull_ThrowsException() {
           Assert.Throws<ArgumentNullException>(() => _formatter.Format(null));
        }

        [Test]
        public void Format_CorrectlyFormatted() {
            var fileName = new FileName {
                DataStream = DataStream.SlowFixation.ToString(),
                DateTime = new DateTime(2018, 10, 12, 5, 20, 3)
            };

            var formattedName = _formatter.Format(fileName);

            Assert.AreEqual("log_SlowFixation_2018_10_12_05_20_03_000", formattedName, "#1");
        }
    }
}
