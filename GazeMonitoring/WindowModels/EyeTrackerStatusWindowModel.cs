using System.ComponentModel;
using System.Runtime.CompilerServices;
using GazeMonitoring.Commands;

namespace GazeMonitoring.WindowModels {
    public class EyeTrackerStatusWindowModel : INotifyPropertyChanged {
        private readonly AwaitableDelegateCommand _startCommand;
        private readonly AwaitableDelegateCommand _stopCommand;
        private bool _isAvailable;
        private string _eyeTrackerName;

        public EyeTrackerStatusWindowModel(AwaitableDelegateCommand startCommand, AwaitableDelegateCommand stopCommand) {
            _startCommand = startCommand;
            _stopCommand = stopCommand;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsAvailable {
            get => _isAvailable;
            set
            {
                if (_isAvailable != value) {
                    _isAvailable = value;
                    OnPropertyChanged();
                    _startCommand.RaiseCanExecuteChanged();
                    _stopCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string EyeTrackerName {
            get => _eyeTrackerName;
            set
            {
                if (_eyeTrackerName != value) {
                    _eyeTrackerName = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
