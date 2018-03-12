using GazeMonitoring.Common.Entities;
using GazeMonitoring.Data;

namespace GazeMonitoring.Common {
    public class GazePointWriter : IGazeDataWriter {
        private readonly IGazeDataRepository _repository;

        public GazePointWriter(IGazeDataRepository repository) {
            _repository = repository;
        }

        public void Write(GazePoint gazePoint) {
            _repository.SaveOne(gazePoint);
        }
    }
}
