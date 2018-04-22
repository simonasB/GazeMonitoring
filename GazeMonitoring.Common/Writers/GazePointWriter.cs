using GazeMonitoring.Data;
using GazeMonitoring.Model;

namespace GazeMonitoring.Common.Writers {
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
