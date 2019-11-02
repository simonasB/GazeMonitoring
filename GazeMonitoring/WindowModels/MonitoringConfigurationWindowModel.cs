using System.Collections.Generic;

namespace GazeMonitoring.WindowModels
{
    public class MonitoringConfigurationWindowModel
    {
        public string Name { get; set; }
        public List<ScreenConfigurationWindowModel> ScreenConfigurations { get; set; }
    }
}
