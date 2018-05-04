using System;
using GazeMonitoring.Data;
using GazeMonitoring.Data.PostgreSQL;
using GazeMonitoring.Model;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.PostgreSQL {
    [TestFixture(Category = TestCategory.UNIT)]
    public class PostgreSQLFileNameFormatterTests_Unit {
        [Test]
        public void Format_NullFileName() {
            var formatter = new PostgreSQLFileNameFormatter();

            Assert.Throws<ArgumentNullException>(() => formatter.Format(null));
        }

        [Test]
        public void Format_CorrectlyFormatted() {
            var formatter = new PostgreSQLFileNameFormatter();

            var formattedName = formatter.Format(new FileName {
                DataStream = "Saccades",
                DateTime = DateTime.UtcNow
            });

            Assert.AreEqual(Constants.SaccadesTempCsvFileName, formattedName, "#1");

            formattedName = formatter.Format(new FileName {
                DataStream = DataStream.SlowFixation.ToString(),
                DateTime = DateTime.Today
            });

            Assert.AreEqual(Constants.GazePointsTempCsvFileName, formattedName, "#2");

        }
    }
}
