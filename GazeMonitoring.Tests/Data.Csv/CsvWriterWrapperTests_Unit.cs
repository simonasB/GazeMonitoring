using System;
using System.IO;
using CsvHelper;
using GazeMonitoring.Data;
using GazeMonitoring.Data.Csv;
using GazeMonitoring.Model;
using Moq;
using NUnit.Framework;

namespace GazeMonitoring.Tests.Data.Csv {
    [TestFixture(Category = TestCategory.UNIT)]
    public class CsvWriterWrapperTests_Unit {
        [Test]
        public void NullConstructorParameters_ThrowsException() {
            Assert.Throws<ArgumentNullException>(() => new CsvWriterWrapper(null, new CsvWriter(TextWriter.Null)));
            Assert.Throws<ArgumentNullException>(() => new CsvWriterWrapper(null, null));
            Assert.Throws<ArgumentNullException>(() => new CsvWriterWrapper(TextWriter.Null, null));
        }
    }
}
