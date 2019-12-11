using System;
using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation
{
    public class MappedFixationPoint : FixationPoint
    {
        public string ScreenConfigurationId { get; set; }
        public string AreaOfInterestName { get; set; }
        public string AreaOfInterestId { get; set; }
    }
}
