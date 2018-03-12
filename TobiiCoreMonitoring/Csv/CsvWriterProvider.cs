using System;
using System.IO;
using CsvHelper;
using GazeMonitoring.Common.Entities;

namespace TobiiCoreMonitoring.Csv {
    public class CsvWriterProvider : IDisposable {
        private readonly FileName _fileName;
        private CsvWriter _csvWriter;
        private TextWriter _textWriter;

        public CsvWriter CsvWriter {
            get
            {
                if (_csvWriter == null) {
                    //_fileName.DateTime = DateTime.Now;
                    _textWriter = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), _fileName.ToString()));
                    _csvWriter = new CsvWriter(_textWriter);
                }
                return _csvWriter;
            }
        }


        public CsvWriterProvider(FileName fileName) {
            _fileName = fileName;
            
        }

        public void Dispose() {
            CsvWriter?.Dispose();
            _textWriter?.Dispose();
        }
    }
}
