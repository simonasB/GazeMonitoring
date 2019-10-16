using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.CustomExample {
    public class ExampleGazeDataRepository : IGazeDataRepository {
        private readonly IExampleWriter _writer;

        public ExampleGazeDataRepository(IExampleWriter writer) {
            _writer = writer;
        }

        public void SaveGazePoint(GazePoint gazePoint) {
            if (gazePoint == null) throw new ArgumentNullException(nameof(gazePoint));
            _writer.Write(gazePoint);
        }

        public void SaveSaccade(Saccade saccade) {
            if (saccade == null) throw new ArgumentNullException(nameof(saccade));
            _writer.Write(saccade);
        }

        public void SaveFixationPoint(FixationPoint point) {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public interface IExampleWriter {
        void Write<T>(T entity);
    }

    public class ExampleWriter : IExampleWriter {
        public void Write<T>(T entity) {
            throw new NotImplementedException();
        }
    }
}
