﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GazeMonitoring.Base;

namespace GazeMonitoring.WindowModels
{
    public class MonitoringConfigurationWindowModel : ValidatableBindableBase
    {
        private string _name;

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public List<ScreenConfigurationWindowModel> ScreenConfigurations { get; set; }
    }
}
