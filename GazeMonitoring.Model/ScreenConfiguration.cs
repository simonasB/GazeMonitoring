using System.Collections.Generic;

namespace GazeMonitoring.Model
{
    public class ScreenConfiguration
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public List<AreaOfInterest> AreasOfInterest { get; set; }
        public string Id { get; set; }
    }
}
