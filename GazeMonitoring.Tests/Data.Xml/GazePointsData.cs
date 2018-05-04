using System.Collections.Generic;
using GazeMonitoring.Model;

namespace GazeMonitoring.Tests.Data.Xml {
    public class GazePointsData {
        public SubjectInfo SubjectInfo { get; set; }
        public List<GazePoint> GazePoints { get; set; }
    }
}
