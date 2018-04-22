using GazeMonitoring.Model;

namespace GazeMonitoring.Common.Calculations {
    public interface ISaccadeCalculator {
        Saccade Calculate(GazePoint previousPoint, GazePoint currentPoint);
    }
}
