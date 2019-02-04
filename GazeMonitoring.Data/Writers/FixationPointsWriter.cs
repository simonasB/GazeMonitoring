using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Writers {
    public class FixationPointsWriter : IGazeDataWriter {
        private readonly IGazeDataRepository _gazeDataRepository;

        public FixationPointsWriter(IGazeDataRepository gazeDataRepository) {
            _gazeDataRepository = gazeDataRepository;
        }

        public void Write(GazePoint gazePoint) {
            var fixationPoint = gazePoint as FixationPoint;
            _gazeDataRepository.SaveFixationPoint(fixationPoint);
        }
    }
}
