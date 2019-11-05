using System;
using GazeMonitoring.Base;

namespace GazeMonitoring.WindowModels
{
    public class ScreenConfigurationWindowModel : ViewModelBase
    {
        private string _name;
        private int _areasOfInterestCount;
        // Initialize with default value 0 minutes and 0 seconds
        private DateTime _duration = new DateTime(2019,1,1, 0,0, 0, DateTimeKind.Utc);

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
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

        public DateTime Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        public string Id { get; set; }
    }
}
