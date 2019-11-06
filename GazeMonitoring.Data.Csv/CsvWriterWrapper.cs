using System;
using System.IO;
using CsvHelper;

namespace GazeMonitoring.Data.Csv {
    public class CsvWriterWrapper : IDisposable {
        private readonly TextWriter _textWriter;

        public CsvWriter CsvWriter { get; }

        public CsvWriterWrapper(TextWriter textWriter, CsvWriter csvWriter) {
            if (textWriter == null) {
                throw new ArgumentNullException(nameof(textWriter));
            }

            if (csvWriter == null) {
                throw new ArgumentNullException(nameof(csvWriter));
            }

            CsvWriter = csvWriter;
            _textWriter = textWriter;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                CsvWriter?.Dispose();
                _textWriter?.Dispose();
            }
        }

        ~CsvWriterWrapper()
        {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
