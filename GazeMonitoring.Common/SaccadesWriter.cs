using GazeMonitoring.Common.Entities;
using GazeMonitoring.Data;

namespace GazeMonitoring.Common {
    public class SaccadesWriter : IGazeDataWriter {
        private readonly IGazeDataRepository _repository;
        private readonly ISaccadeCalculator _saccadeCalculator;
        private GazePoint _previousGazePoint;

        public SaccadesWriter(IGazeDataRepository repository, ISaccadeCalculator saccadeCalculator) {
            _repository = repository;
            _saccadeCalculator = saccadeCalculator;
        }

        public void Write(GazePoint gazePoint) {
            if (_previousGazePoint == null) {
                _previousGazePoint = gazePoint;
            } else {
                _repository.SaveOne(_saccadeCalculator.Calculate(_previousGazePoint, gazePoint));
            }
        }
    }
}
