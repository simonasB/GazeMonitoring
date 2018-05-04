using System;
using GazeMonitoring.Data;
using GazeMonitoring.Data.Xml;
using GazeMonitoring.Model;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.Xml {
    [TestFixture(Category = TestCategory.UNIT)]
    public class XmlFileNameFormatterTests_Unit {
        [Test]
        public void Format_NullFileName() {
            var formatter = new XmlFileNameFormatter();

            Assert.Throws<ArgumentNullException>(() => formatter.Format(null));
        }

        [Test]
        public void Format_CorrectlyFormatted() {
            var formatter = new XmlFileNameFormatter();

            var formattedName = formatter.Format(new FileName {
                DataStream = DataStream.LightlyFilteredGaze.ToString(),
                DateTime = DateTime.UtcNow
            });

            Assert.True(formattedName.EndsWith(".xml"));
        }
    }
}
