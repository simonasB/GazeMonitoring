using GazeMonitoring.Common.Entities;

namespace GazeMonitoring.Common {
    public interface ISaccadeCalculator {
        Saccade Calculate(GazePoint previousPoint, GazePoint currentPoint);
    }
}
