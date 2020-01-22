using System.Collections.Generic;
using System.Linq;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data
{
    public class MultipleSourceGazeDataRepository : IGazeDataRepository
    {
        private readonly List<IGazeDataRepository> _repositories;

        public MultipleSourceGazeDataRepository(IEnumerable<IGazeDataRepository> repositories)
        {
            _repositories = repositories.ToList();
        }

        public void Dispose()
        {
            _repositories.ForEach(o => o.Dispose());
        }

        public void SaveGazePoint(GazePoint gazePoint)
        {
            _repositories.ForEach(o => o.SaveGazePoint(gazePoint));
        }

        public void SaveSaccade(Saccade saccade)
        {
            _repositories.ForEach(o => o.SaveSaccade(saccade));
        }

        public void SaveFixationPoint(FixationPoint fixationPoint)
        {
            _repositories.ForEach(o => o.SaveFixationPoint(fixationPoint));
        }
    }
}
