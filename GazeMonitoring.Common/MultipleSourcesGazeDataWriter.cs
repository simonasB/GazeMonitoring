using System.Collections.Generic;
using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Common {
    public class MultipleSourcesGazeDataWriter : IGazeDataWriter {
        private readonly List<IGazeDataWriter> _gazeDataWriters;

        public MultipleSourcesGazeDataWriter(List<IGazeDataWriter> gazeDataWriters) {
            _gazeDataWriters = gazeDataWriters;
        }
        public void Write(GazePoint gazePoint) {
            foreach (var gazeDataWriter in _gazeDataWriters) {
                gazeDataWriter.Write(gazePoint);
            }
        }
    }
}
