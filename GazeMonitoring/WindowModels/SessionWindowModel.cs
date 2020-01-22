using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GazeMonitoring.Base;
using GazeMonitoring.Model;

namespace GazeMonitoring.WindowModels {
    public class SessionWindowModel : ValidatableBindableBase {
        private string _name;
        private int? _age;
        private string _details;
        private bool _isScreenRecorded;
        private bool _isReportGenerated;
        private List<MonitoringConfiguration> _monitoringConfigurations;

        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Name {
            get { return _name; }
            set => SetProperty(ref _name, value);
        }

        [Required]
        [Range(2, 150)]
        public int? Age {
            get { return _age; }
            set => SetProperty(ref _age, value);
        }

        [StringLength(500)]
        public string Details {
            get { return _details; }
            set => SetProperty(ref _details, value);
        }

        public void ResetErrors() {
            Errors.Clear();
            InvokeErrorsChanged("Name", "Age", "Details");
        }

        public DataStream DataStream { get; set; }
        public MonitoringConfiguration SelectedMonitoringConfiguration { get; set; }

        public List<MonitoringConfiguration> MonitoringConfigurations
        {
            get => _monitoringConfigurations;
            set => SetProperty(ref _monitoringConfigurations, value);
        }

        public bool IsScreenRecorded
        {
            get => _isScreenRecorded;
            set => SetProperty(ref _isScreenRecorded, value);
        }

        public bool IsReportGenerated
        {
            get => _isReportGenerated;
            set => SetProperty(ref _isReportGenerated, value);
        }
    }
}
