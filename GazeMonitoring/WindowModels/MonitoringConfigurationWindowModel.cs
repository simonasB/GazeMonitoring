using System.Collections.Generic;
using GazeMonitoring.Base;

namespace GazeMonitoring.WindowModels
{
    public class MonitoringConfigurationWindowModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public List<ScreenConfigurationWindowModel> ScreenConfigurations { get; set; }
    }
}
