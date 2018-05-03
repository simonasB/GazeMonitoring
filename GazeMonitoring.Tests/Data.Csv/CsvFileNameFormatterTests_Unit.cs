using System;
using GazeMonitoring.Data;
using GazeMonitoring.Data.Csv;
using GazeMonitoring.Model;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.Csv {
    [TestFixture(Category = TestCategory.UNIT)]
    public class CsvFileNameFormatterTests_Unit {
        [Test]
        public void Format_CorrectlyFormatted() {
            var formatter = new CsvFileNameFormatter();

            var formattedName = formatter.Format(new FileName {
                DataStream = DataStream.LightlyFilteredGaze.ToString(),
                DateTime = DateTime.UtcNow
            });

            Assert.True(formattedName.EndsWith(".csv"));
        }
    }
}
