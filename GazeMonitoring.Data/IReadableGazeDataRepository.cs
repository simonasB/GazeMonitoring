using System.Collections.Generic;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data
{
    public interface IReadableGazeDataRepository
    {
        IEnumerable<GazePoint> GetGazePoints();
        IEnumerable<FixationPoint> GetFixationPoints();
        IEnumerable<Saccade> GetSaccades();
    }
}
