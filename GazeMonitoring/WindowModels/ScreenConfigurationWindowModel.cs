using System;
using System.ComponentModel.DataAnnotations;
using GazeMonitoring.Base;

namespace GazeMonitoring.WindowModels
{
    public class ScreenConfigurationWindowModel : ValidatableBindableBase
    {
        private string _name;
        private int _areasOfInterestCount;
        // Initialize with default value 0 minutes and 0 seconds
        private DateTime _duration = new DateTime(2019,1,1, 0,0, 0, DateTimeKind.Utc);

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
        public DateTime Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        public string Id { get; set; }

        public int Number { get; set; }
    }
}
