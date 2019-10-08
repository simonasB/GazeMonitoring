using System.Collections.Generic;

namespace GazeMonitoring.Model
{
    public class MonitoringConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ScreenConfiguration> ScreenConfigurations { get; set; }
    }
}
