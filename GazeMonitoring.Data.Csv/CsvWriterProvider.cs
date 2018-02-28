using System;
using System.IO;
using CsvHelper;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Data.Csv {
    public class CsvWriterProvider<T> : IDisposable where T : IGazeData {
        private readonly FileName _fileName;
        private CsvWriter _csvWriter;
        private TextWriter _textWriter;

        public CsvWriter CsvWriter {
            get
            {
                if (_csvWriter == null) {
                    _fileName.DateTime = DateTime.Now;
                    _textWriter = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), _fileName.ToString()));
                    _csvWriter = new CsvWriter(_textWriter);
                    Init();
                }
                return _csvWriter;
            }
        }

        protected virtual void Init() {
            _csvWriter.WriteHeader<T>();
            _csvWriter.NextRecord();
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
