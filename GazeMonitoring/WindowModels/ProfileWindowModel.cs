using GazeMonitoring.Base;

namespace GazeMonitoring.WindowModels
{
    public class ProfileWindowModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
