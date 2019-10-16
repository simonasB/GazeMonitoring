using System;
using System.Collections.Generic;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Writers {
    public class MultipleSourcesGazeDataWriter : IGazeDataWriter {
        private readonly List<IGazeDataWriter> _gazeDataWriters;

        public MultipleSourcesGazeDataWriter(List<IGazeDataWriter> gazeDataWriters) {
            if (gazeDataWriters == null) {
                throw new ArgumentNullException(nameof(gazeDataWriters));
            }

            _gazeDataWriters = gazeDataWriters;
        }

        public void Write(GazePoint gazePoint) {
            if (gazePoint == null) {
                throw new ArgumentNullException(nameof(gazePoint));
            }

            foreach (var gazeDataWriter in _gazeDataWriters) {
                gazeDataWriter.Write(gazePoint);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _gazeDataWriters.ForEach(w => w.Dispose());
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MultipleSourcesGazeDataWriter()
        {
            Dispose(false);
        }
    }
}
