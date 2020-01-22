using System.Collections.Generic;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data
{
    public interface ICurrentSessionData
    {
        IEnumerable<GazePoint> GetGazePoints();

        IEnumerable<FixationPoint> GetFixationPoints();

        IEnumerable<Saccade> GetSaccades();
    }
}
