using System.Collections.Generic;
using GazeMonitoring.Model;

namespace GazeMonitoring.Tests.Data.Xml {
    public class SaccadesData {
        public SubjectInfo SubjectInfo { get; set; }
        public List<Saccade> Saccades { get; set; }
    }
}
