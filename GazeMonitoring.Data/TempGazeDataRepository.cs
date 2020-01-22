using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data
{
    public class TempGazeDataRepository : IGazeDataRepository, IReadableGazeDataRepository
    {
        private readonly ITempDataConfiguration _configuration;
        private readonly List<GazePoint> _gazePoints;
        private readonly List<FixationPoint> _fixationPoints;
        private readonly List<Saccade> _saccades;

        public TempGazeDataRepository(ITempDataConfiguration configuration)
        {
            _configuration = configuration;
            _gazePoints = new List<GazePoint>();
            _fixationPoints = new List<FixationPoint>();
            _saccades = new List<Saccade>();
        }

        public void Dispose()
        {
            var gazePointWriter = new XmlSerializer(typeof(List<GazePoint>));
            var fixationPointWriter = new XmlSerializer(typeof(List<FixationPoint>));
            var saccadesWriter = new XmlSerializer(typeof(List<Saccade>));

            using (var file = File.Create(_configuration.GazePointsFilePath))
            {
                gazePointWriter.Serialize(file, _gazePoints);
            }
            using (var file = File.Create(_configuration.FixationPointsFilePath))
            {
                fixationPointWriter.Serialize(file, _fixationPoints);
            }
            using (var file = File.Create(_configuration.SaccadesFilePath))
            {
                saccadesWriter.Serialize(file, _saccades);
            }
        }

        public void SaveGazePoint(GazePoint gazePoint)
        {
            _gazePoints.Add(gazePoint);
        }

        public void SaveSaccade(Saccade saccade)
        {
            _saccades.Add(saccade);
        }

        public void SaveFixationPoint(FixationPoint fixationPoint)
        {
            _fixationPoints.Add(fixationPoint);
        }

        public IEnumerable<GazePoint> GetGazePoints()
        {
            return _gazePoints;
        }

        public IEnumerable<FixationPoint> GetFixationPoints()
        {
            return _fixationPoints;
        }

        public IEnumerable<Saccade> GetSaccades()
        {
            return _saccades;
        }
    }
}
