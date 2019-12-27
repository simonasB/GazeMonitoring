using System;
using System.ComponentModel.DataAnnotations;
using GazeMonitoring.Base;

namespace GazeMonitoring.WindowModels
{
    public class ScreenConfigurationWindowModel : ValidatableBindableBase
    {
        private string _name;
        private int _areasOfInterestCount;
        private DateTime? _duration;

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int AreasOfInterestCount
        {
            get => _areasOfInterestCount;
            set
            {
                _areasOfInterestCount = value;
                OnPropertyChanged();
            }
        }

        [Required]
        public DateTime? Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        public string Id { get; set; }

        public int Number { get; set; }
    }
}
