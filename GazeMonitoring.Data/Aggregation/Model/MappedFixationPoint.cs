﻿using GazeMonitoring.Model;

namespace GazeMonitoring.Data.Aggregation.Model
{
    public class MappedFixationPoint : FixationPoint
    {
        public string ScreenConfigurationId { get; set; }
        public string ScreenConfigurationName { get; set; }
        public string AreaOfInterestName { get; set; }
        public string AreaOfInterestId { get; set; }
    }
}
