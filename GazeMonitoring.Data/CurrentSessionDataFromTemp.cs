using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data
{
    public class CurrentSessionDataFromTemp : ICurrentSessionData
    {
        private readonly ITempDataConfiguration _configuration;

        public CurrentSessionDataFromTemp(ITempDataConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<GazePoint> GetGazePoints()
        {
            var gazePointWriter = new XmlSerializer(typeof(List<GazePoint>));
            using (var file = new StreamReader(_configuration.GazePointsFilePath))
            {
                return (IEnumerable<GazePoint>)gazePointWriter.Deserialize(file);
            }
        }

        public IEnumerable<FixationPoint> GetFixationPoints()
        {
            var fixationPointWriter = new XmlSerializer(typeof(List<FixationPoint>));
            using (var file = new StreamReader(_configuration.FixationPointsFilePath))
            {
                return (IEnumerable<FixationPoint>)fixationPointWriter.Deserialize(file);
            }
        }

        public IEnumerable<Saccade> GetSaccades()
        {
            var saccadesWriter = new XmlSerializer(typeof(List<Saccade>));
            using (var file = new StreamReader(_configuration.SaccadesFilePath))
            {
                return (IEnumerable<Saccade>)saccadesWriter.Deserialize(file);
            }
        }
    }
}
